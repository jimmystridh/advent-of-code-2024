import scala.io.StdIn
import scala.util.matching.Regex

object MullItOver {
  def main(args: Array[String]): Unit = {
    // Read the entire input from standard input
    val input = Iterator.continually(StdIn.readLine()).takeWhile(_ != null).mkString("\n")

    // Define the regex pattern to match 'mul(X,Y)', 'do()', and 'don't()' instructions
    val pattern: Regex = """mul\(([0-9]{1,3}),([0-9]{1,3})\)|do\(\)|don't\(\)""".r

    // Find all matches in the input string
    val matches = pattern.findAllMatchIn(input).toList

    // Part One: Sum of all valid 'mul' instructions
    val partOneTotal: Int = matches.collect {
      case m if m.matched.startsWith("mul") =>
        val x = m.group(1).toInt
        val y = m.group(2).toInt
        x * y
    }.sum

    // Part Two: Sum of enabled 'mul' instructions considering 'do()' and 'don't()'
    val (_, partTwoTotal) = matches.foldLeft((true, 0)) { case ((mulEnabled, total), m) =>
      m.matched match {
        case "do()"     => (true, total)
        case "don't()"  => (false, total)
        case mul if mul.startsWith("mul") =>
          val x = m.group(1).toInt
          val y = m.group(2).toInt
          if (mulEnabled)
            (mulEnabled, total + x * y)
          else
            (mulEnabled, total)
        case _ => (mulEnabled, total)
      }
    }

    // Output the results
    println(partOneTotal)
    println(partTwoTotal)
  }
}