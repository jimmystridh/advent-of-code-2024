def is_safe(levels):
    """
    Determine if a report is safe based on the levels.

    A report is safe if:
    - The levels are either all increasing or all decreasing.
    - Any two adjacent levels differ by at least one and at most three.
    """
    if len(levels) < 2:
        return False  # Not enough levels to compare
    diffs = [b - a for a, b in zip(levels, levels[1:])]
    all_positive = all(d > 0 for d in diffs)
    all_negative = all(d < 0 for d in diffs)
    diffs_valid = all(1 <= abs(d) <= 3 for d in diffs)
    return diffs_valid and (all_positive or all_negative)

def main():
    import sys

    # Read input reports
    reports = []
    for line in sys.stdin:
        line = line.strip()
        if line:
            levels = list(map(int, line.split()))
            reports.append(levels)

    safe_count_part1 = 0
    safe_count_part2 = 0

    # Analyze each report
    for levels in reports:
        if is_safe(levels):
            # Report is safe under original rules (Part One)
            safe_count_part1 += 1
            safe_count_part2 += 1
        else:
            # For Part Two, try removing one level at a time
            found_safe = False
            for i in range(len(levels)):
                new_levels = levels[:i] + levels[i+1:]
                if is_safe(new_levels):
                    found_safe = True
                    break
            if found_safe:
                safe_count_part2 += 1

    # Output results
    print(f"Part One: {safe_count_part1}")
    print(f"Part Two: {safe_count_part2}")

if __name__ == "__main__":
    main()