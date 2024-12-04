from sys import stdin
from itertools import product

def read_grid():
    grid = [list(line.strip()) for line in stdin if line.strip()]
    return grid

def count_xmas(grid):
    num_rows, num_cols = len(grid), len(grid[0])
    word = 'XMAS'
    directions = [(-1, 0), (-1, 1), (0, 1), (1, 1),
                  (1, 0), (1, -1), (0, -1), (-1, -1)]

    def is_valid(x, y):
        return 0 <= x < num_rows and 0 <= y < num_cols

    def check_word(x, y, dx, dy):
        positions = [(x + i * dx, y + i * dy) for i in range(len(word))]
        if all(is_valid(px, py) for px, py in positions):
            letters = [grid[px][py] for px, py in positions]
            return ''.join(letters) == word
        return False

    count = sum(
        1
        for x, y in product(range(num_rows), range(num_cols))
        for dx, dy in directions
        if check_word(x, y, dx, dy)
    )
    return count

def count_x_mas(grid):
    num_rows, num_cols = len(grid), len(grid[0])
    patterns = {'MAS', 'SAM'}
    count = 0

    def is_valid(x, y):
        return 0 <= x < num_rows and 0 <= y < num_cols

    for x in range(num_rows - 2):
        for y in range(num_cols - 2):
            diag1 = [(x, y), (x + 1, y + 1), (x + 2, y + 2)]
            diag2 = [(x, y + 2), (x + 1, y + 1), (x + 2, y)]
            if all(is_valid(px, py) for px, py in diag1 + diag2):
                letters1 = [grid[px][py] for px, py in diag1]
                letters2 = [grid[px][py] for px, py in diag2]
                word1 = ''.join(letters1)
                word2 = ''.join(letters2)
                word1_rev = word1[::-1]
                word2_rev = word2[::-1]
                if ((word1 in patterns or word1_rev in patterns) and
                    (word2 in patterns or word2_rev in patterns) and
                    letters1[1] == 'A' and letters2[1] == 'A'):
                    count += 1
    return count

def main():
    grid = read_grid()
    part_one_count = count_xmas(grid)
    part_two_count = count_x_mas(grid)
    print(f"Part One: {part_one_count}")
    print(f"Part Two: {part_two_count}")

if __name__ == "__main__":
    main()