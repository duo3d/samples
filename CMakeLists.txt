cmake_minimum_required (VERSION 2.8) 
project (DUO_SAMPLES)
set(CMAKE_BUILD_TYPE Release)

# Add DUO include and lib
include_directories(${CMAKE_CURRENT_SOURCE_DIR}/include) 
link_directories(${CMAKE_CURRENT_SOURCE_DIR}/lib ${CMAKE_CURRENT_SOURCE_DIR}/bin)

# Set output directory for binaries
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY_RELEASE ${CMAKE_BINARY_DIR}/bin )

add_subdirectory(Sample-01)
add_subdirectory(Sample-02)
add_subdirectory(Sample-03)
add_subdirectory(Sample-04)
add_subdirectory(Sample-05)
add_subdirectory(Sample-06)
