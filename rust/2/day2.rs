use std::io::{self, BufRead};

/// Determines if a report is safe based on the levels.
/// A report is safe if:
/// - The levels are either all increasing or all decreasing.
/// - Any two adjacent levels differ by at least one and at most three.
fn is_safe_report(levels: &[i32]) -> bool {
    if levels.len() < 2 {
        return false; // Not enough levels to compare
    }

    let diffs: Vec<i32> = levels.windows(2).map(|w| w[1] - w[0]).collect();

    let all_increasing = diffs.iter().all(|&d| d > 0);
    let all_decreasing = diffs.iter().all(|&d| d < 0);
    let diffs_valid = diffs.iter().all(|&d| d.abs() >= 1 && d.abs() <= 3);

    diffs_valid && (all_increasing || all_decreasing)
}

fn main() {
    let stdin = io::stdin();
    let lines = stdin.lock().lines();

    let mut safe_count_part1 = 0;
    let mut safe_count_part2 = 0;

    for line in lines {
        if let Ok(line) = line {
            let line = line.trim();
            if line.is_empty() {
                continue;
            }

            // Parse the levels from the line
            let levels: Vec<i32> = line
                .split_whitespace()
                .filter_map(|s| s.parse::<i32>().ok())
                .collect();

            if is_safe_report(&levels) {
                // Report is safe under original rules (Part One)
                safe_count_part1 += 1;
                safe_count_part2 += 1;
            } else {
                // Try removing one level at a time for Part Two
                let mut found_safe = false;
                for i in 0..levels.len() {
                    let mut new_levels = levels.clone();
                    new_levels.remove(i);
                    if is_safe_report(&new_levels) {
                        found_safe = true;
                        break;
                    }
                }
                if found_safe {
                    safe_count_part2 += 1;
                }
            }
        }
    }

    println!("Part One: {}", safe_count_part1);
    println!("Part Two: {}", safe_count_part2);
}
