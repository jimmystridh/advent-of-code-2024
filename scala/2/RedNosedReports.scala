object RedNosedReports {
  def main(args: Array[String]): Unit = {
    import scala.io.StdIn

    // Read input reports
    val reports = Iterator.continually(StdIn.readLine())
      .takeWhile(_ != null)
      .map(_.trim)
      .filter(_.nonEmpty)
      .map(line => line.split("\\s+").map(_.toInt))
      .toList

    val (safeCountPart1, safeCountPart2) = reports.foldLeft((0, 0)) {
      case ((count1, count2), levels) =>
        if (isSafeReport(levels)) {
          // Safe under original rules
          (count1 + 1, count2 + 1)
        } else {
          // Try removing one level at a time for Part Two
          val foundSafe = levels.indices.exists { i =>
            val newLevels = levels.patch(i, Nil, 1)
            isSafeReport(newLevels)
          }
          if (foundSafe) (count1, count2 + 1) else (count1, count2)
        }
    }

    println(s"Part One: $safeCountPart1")
    println(s"Part Two: $safeCountPart2")
  }

  def isSafeReport(levels: Array[Int]): Boolean = {
    if (levels.length < 2) return false

    val diffs = levels.sliding(2).map { case Array(a, b) => b - a }.toArray

    val allIncreasing = diffs.forall(_ > 0)
    val allDecreasing = diffs.forall(_ < 0)
    val diffsValid = diffs.forall(d => math.abs(d) >= 1 && math.abs(d) <= 3)

    diffsValid && (allIncreasing || allDecreasing)
  }
}