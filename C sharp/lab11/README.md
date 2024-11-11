In this lab, I worked on three tasks related to multithreading and data streams in C#. 

Each task involved using different multithreading techniques and data manipulation through streams.

Key Tasks:
1. Calculate Binomial Coefficient (Newton's Symbol) Concurrently:
  Implement three versions of the calculation using:
  - Task and Task<T>
  - Delegates for asynchronous method calls
  - Async-Await method
2. Calculate Fibonacci Sequence with Progress Reporting:
  Calculate the i-th Fibonacci number sequentially using a BackgroundWorker.
  After calculating each Fibonacci number, update the progress bar (ProgressBar).
  Add a small delay after each calculation to simulate processing time.
3. Compress/Decompress Files Using GZipStream:
  Compress and decompress files in a selected directory using GZipStream.
  For each file, create a separate archive with the .gz extension, compressing them concurrently in a parallel loop.
  Implement file processing with a FolderBrowserDialog to select the target folder.
