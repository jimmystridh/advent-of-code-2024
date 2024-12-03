#include <iostream>
#include <vector>
#include <unordered_map>
#include <thrust/device_vector.h>
#include <thrust/host_vector.h>
#include <thrust/sort.h>
#include <thrust/transform.h>
#include <thrust/reduce.h>
#include <thrust/functional.h>
#include <thrust/unique.h>
#include <thrust/gather.h>
#include <thrust/binary_search.h>
#include <thrust/iterator/constant_iterator.h>
#include <thrust/iterator/counting_iterator.h>

struct abs_diff
{
    __host__ __device__
    int operator()(const int& a, const int& b) const
    {
        return abs(a - b);
    }
};

// Functor to replace the lambda
struct MapCountsFunctor
{
    int* d_indices;
    int* d_unique_keys;
    int* d_counts;
    int num_unique;
    int* d_left;

    MapCountsFunctor(int* indices, int* unique_keys, int* counts, int num_unique, int* left)
        : d_indices(indices), d_unique_keys(unique_keys), d_counts(counts), num_unique(num_unique), d_left(left) {}

    __device__
    int operator()(int i) const
    {
        int idx = d_indices[i];
        if (idx < num_unique && d_unique_keys[idx] == d_left[i])
        {
            return d_counts[idx];
        }
        else
        {
            return 0;
        }
    }
};

int main()
{
    // Read input data
    std::vector<int> left_list;
    std::vector<int> right_list;

    int left_num, right_num;
    while (std::cin >> left_num >> right_num)
    {
        left_list.push_back(left_num);
        right_list.push_back(right_num);
    }

    int N = left_list.size();

    // Transfer data to device (GPU)
    thrust::device_vector<int> d_left(left_list);
    thrust::device_vector<int> d_right(right_list);

    // Part One: Total Distance Calculation
    // Sort both lists on the device
    thrust::sort(d_left.begin(), d_left.end());
    thrust::sort(d_right.begin(), d_right.end());

    // Compute absolute differences in parallel
    thrust::device_vector<int> d_differences(N);
    thrust::transform(d_left.begin(), d_left.end(), d_right.begin(), d_differences.begin(), abs_diff());

    // Compute the total distance using parallel reduction
    int total_distance = thrust::reduce(d_differences.begin(), d_differences.end(), 0, thrust::plus<int>());

    std::cout << "Total distance: " << total_distance << std::endl;

    // Part Two: Similarity Score Calculation

    // Step 1: Sort the right list to prepare for counting
    thrust::sort(d_right.begin(), d_right.end());

    // Step 2: Count the occurrences of each unique number in the right list
    thrust::device_vector<int> d_unique_keys(N);
    thrust::device_vector<int> d_counts(N);

    auto new_end = thrust::reduce_by_key(
        d_right.begin(), d_right.end(),
        thrust::constant_iterator<int>(1),
        d_unique_keys.begin(),
        d_counts.begin()
    );

    int num_unique = new_end.first - d_unique_keys.begin();

    // Resize vectors to the number of unique elements
    d_unique_keys.resize(num_unique);
    d_counts.resize(num_unique);

    // Step 3: Map counts to the left list numbers
    // For each element in d_left, find its index in d_unique_keys
    // Use thrust::lower_bound since d_unique_keys is sorted
    thrust::device_vector<int> d_indices(N);
    thrust::lower_bound(
        d_unique_keys.begin(), d_unique_keys.end(),
        d_left.begin(), d_left.end(),
        d_indices.begin()
    );

    // Create a functor to map counts
    MapCountsFunctor map_counts(
        thrust::raw_pointer_cast(d_indices.data()),
        thrust::raw_pointer_cast(d_unique_keys.data()),
        thrust::raw_pointer_cast(d_counts.data()),
        num_unique,
        thrust::raw_pointer_cast(d_left.data())
    );

    // Create a device vector to hold counts corresponding to the left list numbers
    thrust::device_vector<int> d_left_counts(N);

    // Use thrust::transform with the functor
    thrust::transform(
        thrust::make_counting_iterator(0),
        thrust::make_counting_iterator(N),
        d_left_counts.begin(),
        map_counts
    );

    // Compute the similarity score
    // For each number in the left list, multiply it by its count in the right list
    thrust::device_vector<int> d_similarity(N);
    thrust::transform(
        d_left.begin(), d_left.end(),
        d_left_counts.begin(),
        d_similarity.begin(),
        thrust::multiplies<int>()
    );

    // Sum up the similarity scores
    int similarity_score = thrust::reduce(d_similarity.begin(), d_similarity.end(), 0, thrust::plus<int>());

    std::cout << "Similarity score: " << similarity_score << std::endl;

    return 0;
}