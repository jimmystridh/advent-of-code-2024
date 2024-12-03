object HistorianHysteria {

  def main(args: Array[String]): Unit = {
    // Read input
    val (leftList, rightList) = readInput()

    // Part One
    val totalDistance = calculateTotalDistance(leftList, rightList)
    println(s"Total distance: $totalDistance")

    // Part Two
    val similarityScore = calculateSimilarityScore(leftList, rightList)
    println(s"Similarity score: $similarityScore")
  }

  def readInput(): (List[Int], List[Int]) = {
    import scala.io.StdIn

    var leftList = List.empty[Int]
    var rightList = List.empty[Int]

    var line: String = null
    while ({ line = StdIn.readLine(); line != null }) {
      if (line.trim.nonEmpty) {
        val tokens = line.trim.split("\\s+")
        if (tokens.length != 2) {
          throw new IllegalArgumentException(s"Invalid input line: $line")
        }
        val leftNum = tokens(0).toInt
        val rightNum = tokens(1).toInt
        leftList ::= leftNum
        rightList ::= rightNum
      }
    }
    (leftList.reverse, rightList.reverse)
  }

  def calculateTotalDistance(leftList: List[Int], rightList: List[Int]): Int = {
    val sortedLeft = leftList.sorted
    val sortedRight = rightList.sorted
    val distances = sortedLeft.zip(sortedRight).map { case (a, b) => math.abs(a - b) }
    distances.sum
  }

  def calculateSimilarityScore(leftList: List[Int], rightList: List[Int]): Int = {
    val rightCounts = rightList.groupBy(identity).view.mapValues(_.size).toMap
    leftList.map(num => num * rightCounts.getOrElse(num, 0)).sum
  }
}