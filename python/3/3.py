import sys
import re

def main():
    # Read the input from standard input
    input_data = sys.stdin.read()

    # Define the regex pattern to match 'mul(X,Y)', 'do()', and 'don't()' instructions
    pattern = r"mul\(([0-9]{1,3}),([0-9]{1,3})\)|do\(\)|don't\(\)"
    matches = re.findall(pattern, input_data)

    # Part One: Sum of all valid 'mul' instructions
    part_one_total = 0
    for match in matches:
        if match[0] and match[1]:
            x = int(match[0])
            y = int(match[1])
            part_one_total += x * y

    # Part Two: Sum considering 'do()' and 'don't()' instructions
    mul_enabled = True
    part_two_total = 0

    # Find all matches with their positions to process them in order
    iterator = re.finditer(pattern, input_data)
    for match in iterator:
        value = match.group()
        if value == "do()":
            mul_enabled = True
        elif value == "don't()":
            mul_enabled = False
        elif value.startswith("mul"):
            x = int(match.group(1))
            y = int(match.group(2))
            if mul_enabled:
                part_two_total += x * y

    # Output the results
    print(part_one_total)
    print(part_two_total)

if __name__ == "__main__":
    main()