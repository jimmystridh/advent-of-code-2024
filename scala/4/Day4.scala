object CeresSearch extends App {
  // Read the grid from standard input
  val grid: Array[Array[Char]] = io.Source.stdin.getLines().toArray.map(_.toCharArray)

  // Part One: Count occurrences of "XMAS"
  val partOneCount = countXMAS(grid)
  println(s"Part One: $partOneCount")

  // Part Two: Count occurrences of "X-MAS" patterns
  val partTwoCount = countXMAS_X(grid)
  println(s"Part Two: $partTwoCount")

  // Function to count all occurrences of "XMAS" in any direction
  def countXMAS(grid: Array[Array[Char]]): Int = {
    val numRows = grid.length
    val numCols = grid(0).length
    val word = "XMAS"
    val directions = Seq(
      (-1, 0),    // North
      (-1, 1),    // Northeast
      (0, 1),     // East
      (1, 1),     // Southeast
      (1, 0),     // South
      (1, -1),    // Southwest
      (0, -1),    // West
      (-1, -1)    // Northwest
    )

    def isValid(x: Int, y: Int): Boolean =
      x >= 0 && x < numRows && y >= 0 && y < numCols

    def checkWord(x: Int, y: Int, dx: Int, dy: Int): Boolean = {
      val positions = for (i <- 0 until word.length) yield (x + i * dx, y + i * dy)
      if (positions.forall { case (xi, yi) => isValid(xi, yi) }) {
        val letters = positions.map { case (xi, yi) => grid(xi)(yi) }
        letters.mkString == word
      } else {
        false
      }
    }

    val count = (for {
      x <- 0 until numRows
      y <- 0 until numCols
      (dx, dy) <- directions
      if checkWord(x, y, dx, dy)
    } yield 1).sum

    count
  }

  // Function to count all "X-MAS" patterns
  def countXMAS_X(grid: Array[Array[Char]]): Int = {
    val numRows = grid.length
    val numCols = grid(0).length
    val patterns = Set("MAS", "SAM")

    def isValid(x: Int, y: Int): Boolean =
      x >= 0 && x < numRows && y >= 0 && y < numCols

    val count = (for {
      x <- 0 until numRows - 2
      y <- 0 until numCols - 2
      diag1 = Seq((x, y), (x + 1, y + 1), (x + 2, y + 2))
      diag2 = Seq((x, y + 2), (x + 1, y + 1), (x + 2, y))
      if diag1.forall { case (xi, yi) => isValid(xi, yi) } &&
         diag2.forall { case (xi, yi) => isValid(xi, yi) }
      letters1 = diag1.map { case (xi, yi) => grid(xi)(yi) }
      letters2 = diag2.map { case (xi, yi) => grid(xi)(yi) }
      word1 = letters1.mkString
      word2 = letters2.mkString
      word1Rev = letters1.reverse.mkString
      word2Rev = letters2.reverse.mkString
      if (patterns.contains(word1) || patterns.contains(word1Rev)) &&
         (patterns.contains(word2) || patterns.contains(word2Rev)) &&
         letters1(1) == 'A' && letters2(1) == 'A'
    } yield 1).sum

    count
  }
}