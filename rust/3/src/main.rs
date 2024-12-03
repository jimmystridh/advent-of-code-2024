use std::io::{self, Read};
use regex::Regex;

fn main() {
    // Read the entire input from stdin
    let mut input = String::new();
    io::stdin().read_to_string(&mut input).expect("Failed to read input");

    // Define the regex pattern to match 'mul(X,Y)', 'do()', and 'don't()' instructions
    let pattern = r"mul\(([0-9]{1,3}),([0-9]{1,3})\)|do\(\)|don't\(\)";
    let re = Regex::new(pattern).unwrap();

    // Find all matches in the input string
    let matches: Vec<_> = re.captures_iter(&input).collect();

    // Part One: Sum of all valid 'mul' instructions
    let part_one_total: i32 = matches.iter()
        .filter_map(|caps| {
            if let Some(mul_match) = caps.get(0) {
                if mul_match.as_str().starts_with("mul") {
                    let x: i32 = caps[1].parse().unwrap();
                    let y: i32 = caps[2].parse().unwrap();
                    return Some(x * y);
                }
            }
            None
        })
        .sum();

    // Part Two: Sum of enabled 'mul' instructions considering 'do()' and 'don't()'
    let mut mul_enabled = true;
    let mut part_two_total = 0;

    for caps in matches {
        if let Some(full_match) = caps.get(0) {
            let value = full_match.as_str();
            if value == "do()" {
                mul_enabled = true;
            } else if value == "don't()" {
                mul_enabled = false;
            } else if value.starts_with("mul") {
                let x: i32 = caps[1].parse().unwrap();
                let y: i32 = caps[2].parse().unwrap();
                if mul_enabled {
                    part_two_total += x * y;
                }
            }
        }
    }

    // Output the results
    println!("{}", part_one_total);  // Output for Part One
    println!("{}", part_two_total);  // Output for Part Two
}