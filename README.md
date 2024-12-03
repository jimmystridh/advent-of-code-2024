# Advent of Code Multi-Language Implementation using o1-preview

This repository contains implementations of Advent of Code problems in multiple programming languages using a consistent, functional-first approach. Each solution demonstrates idiomatic usage of the target language while maintaining similar algorithmic approaches across implementations.

Implementations are done **entirely by OpenAI o1-preview**, and so far it has solved these basically on the first try, albeit with some compilation problems it had to fix for the Cuda implementation.

## Languages Implemented

- Python
- TypeScript
- Rust
- Scala
- C# (.NET 9.0)
- CUDA C++

## Project Structure

```
advent/
├── python/
│   ├── 1/ (Historian Hysteria)
│   ├── 2/ (Red-Nosed Reports)
│   └── 3/ (Mull It Over)
├── typescript/
│   ├── 1/
│   ├── 2/
│   └── 3/
├── rust/
│   ├── day1/
│   ├── day2/
│   └── day3/
├── scala/
│   ├── 1/
│   ├── 2/
│   └── 3/
├── csharp/
│   ├── Day1/
│   ├── Day2/
│   └── 3/
└── cuda/
    ├── 1/
    ├── 2/
    └── 3/
```

## Problems

### Day 1: Historian Hysteria
A comparison-based puzzle involving two lists of numbers. The solution requires:
- Calculating total distance between sorted lists
- Computing similarity scores based on frequency matching

### Day 2: Red-Nosed Reports
Analysis of numerical sequences with strict rules about differences between adjacent numbers:
- Validation of increasing/decreasing sequences
- Distance constraints between adjacent values
- Optional element removal for sequence validation

### Day 3: Mull It Over
A pattern-matching and calculation problem with stateful processing:
- Regular expression parsing of commands
- Stateful processing of enable/disable flags
- Parallel computation capabilities (in CUDA implementation)

## Implementation Highlights

### Python
- Clean, readable implementations using modern Python features
- Effective use of list comprehensions and functional programming concepts
- Type hints for improved code clarity

### TypeScript
- Strong typing with TypeScript interfaces
- Functional programming approach using array methods
- Async/await for file operations

### Rust
- Memory-safe implementations with zero-cost abstractions
- Effective use of iterators and functional programming
- Pattern matching for control flow

### Scala
- Pure functional programming approach
- Immutable data structures
- Pattern matching and case classes

### C# (.NET 9.0)
- Modern C# features including pattern matching
- LINQ for functional-style operations
- Strong typing and null safety

### CUDA C++
- Parallel processing for computationally intensive operations
- Efficient memory management between host and device
- Use of Thrust library for parallel algorithms

## Key Features

1. **Consistent Algorithms**: Similar algorithmic approaches across languages while leveraging each language's strengths.

2. **Functional Programming**: Emphasis on immutable data and pure functions where appropriate.

3. **Performance Optimization**: 
   - CUDA implementations for parallel processing
   - Efficient data structures and algorithms
   - Memory-conscious implementations

4. **Modern Language Features**:
   - Pattern matching
   - Type inference
   - Null safety
   - Async/await patterns

5. **Clean Code Principles**:
   - Clear variable and function naming
   - Comprehensive error handling
   - Consistent code style within each language
   - Well-documented functions and modules

## Building and Running

Each language implementation includes its own build and run instructions appropriate for that ecosystem:

### Python
```bash
cd python/<day>
python3 <filename>.py < input.txt
```

### TypeScript
```bash
cd typescript/<day>
ts-node <filename>.ts < input.txt
```

### Rust
```bash
cd rust/<day>
cargo run < input.txt
```

### Scala
```bash
cd scala/<day>
scala <filename>.scala < input.txt
```

### C#
```bash
cd csharp/<day>
dotnet run < input.txt
```

### CUDA
```bash
cd cuda/<day>
nvcc <filename>.cu -o solution
./solution < input.txt
```

## Performance Considerations

- The CUDA implementations provide significant speedup for larger datasets
- Each language implementation is optimized for its specific runtime environment
- Memory usage is carefully considered across all implementations

## Contributing

Feel free to submit improvements or additional language implementations. Please maintain:
1. Consistent algorithm approaches
2. Clear documentation
3. Idiomatic usage of each language
4. Comprehensive error handling
5. Performance considerations

## License

This project is open source and available under the MIT License.
