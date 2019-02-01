# GPUvsCPUgui

C# aplication written for my engineering thesis, to prove how CUDA-enabled GPU outperforms CPU in tasks that can be parallelized.
Application generates random pairs of (x, y) points and computes distance between them and angle between line and x axis.
Computations are performed for different sizes of pair collections. Application measures time of computation for each collection,
on CPU and GPU, and dipslays it on linear chart. X axis shows number of pairs in collection, and Y axis shows computation time in milliseconds.

Application uses OxyPlot library for ploting, and AleaGPU library for CUDA computations.

Application reguires CUDA-enabled GPU to work properly.
Libraries used in this project have problems with nonstandard unicode characters (for example polish letters).
Make sure that your user folder name does not includes nonstandard unicode characters.
