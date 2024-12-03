use std::collections::HashMap;
use std::io::{self, BufRead};

fn main() {
    // Read input
    let (left_list, right_list) = read_input();

    // Part One: Calculate Total Distance
    let total_distance = calculate_total_distance(&left_list, &right_list);
    println!("Total distance: {}", total_distance);

    // Part Two: Calculate Similarity Score
    let similarity_score = calculate_similarity_score(&left_list, &right_list);
    println!("Similarity score: {}", similarity_score);
}

fn read_input() -> (Vec<i32>, Vec<i32>) {
    let stdin = io::stdin();
    let mut left_list = Vec::new();
    let mut right_list = Vec::new();

    for line in stdin.lock().lines() {
        let line = line.expect("Failed to read line");
        if line.trim().is_empty() {
            continue; // Skip empty lines
        }
        let tokens: Vec<&str> = line.trim().split_whitespace().collect();
        if tokens.len() != 2 {
            panic!("Invalid input line: {}", line);
        }
        let left_num = tokens[0].parse::<i32>().expect("Invalid number");
        let right_num = tokens[1].parse::<i32>().expect("Invalid number");
        left_list.push(left_num);
        right_list.push(right_num);
    }

    (left_list, right_list)
}

fn calculate_total_distance(left_list: &Vec<i32>, right_list: &Vec<i32>) -> i32 {
    let mut sorted_left = left_list.clone();
    let mut sorted_right = right_list.clone();

    sorted_left.sort();
    sorted_right.sort();

    let distances: Vec<i32> = sorted_left
        .iter()
        .zip(sorted_right.iter())
        .map(|(a, b)| (a - b).abs())
        .collect();

    distances.iter().sum()
}

fn calculate_similarity_score(left_list: &Vec<i32>, right_list: &Vec<i32>) -> i32 {
    let mut right_counts: HashMap<i32, i32> = HashMap::new();

    for &num in right_list {
        *right_counts.entry(num).or_insert(0) += 1;
    }

    left_list
        .iter()
        .map(|&num| num * right_counts.get(&num).unwrap_or(&0))
        .sum()
}