from collections import Counter

def main():
    # Read input
    left_list = []
    right_list = []
    try:
        while True:
            line = input()
            if not line.strip():
                continue  # Skip empty lines
            tokens = line.strip().split()
            if len(tokens) != 2:
                raise ValueError(f"Invalid input line: {line}")
            left_num = int(tokens[0])
            right_num = int(tokens[1])
            left_list.append(left_num)
            right_list.append(right_num)
    except EOFError:
        pass

    # Part One
    total_distance = calculate_total_distance(left_list, right_list)
    print(f"Total distance: {total_distance}")

    # Part Two
    similarity_score = calculate_similarity_score(left_list, right_list)
    print(f"Similarity score: {similarity_score}")

def calculate_total_distance(left_list, right_list):
    """
    Calculate the total distance between the sorted left and right lists.
    """
    sorted_left = sorted(left_list)
    sorted_right = sorted(right_list)
    distances = [abs(a - b) for a, b in zip(sorted_left, sorted_right)]
    total_distance = sum(distances)
    return total_distance

def calculate_similarity_score(left_list, right_list):
    """
    Calculate the similarity score based on the frequency of numbers
    from the left list appearing in the right list.
    """
    right_counts = Counter(right_list)
    similarity_score = sum(num * right_counts.get(num, 0) for num in left_list)
    return similarity_score

if __name__ == "__main__":
    main()