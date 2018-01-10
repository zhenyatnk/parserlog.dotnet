cmake_minimum_required(VERSION 3.5 FATAL_ERROR)

set(_project_name parserlog.native)

#
# Parameters repository
#
set(${_project_name}_repo "https://github.com/zhenyatnk/parserlog.native.git")
set(${_project_name}_tag "master")

set(${_project_name}_main_dir ${CMAKE_CURRENT_BINARY_DIR})

set(${_project_name}_source_dir   ${_project_name}_src)
set(${_project_name}_build_dir    ${_project_name}_build)
set(${_project_name}_download_dir ${_project_name}_download)

if(INSTALL_DIRECTORY)
    set(CMAKE_INSTALL_PREFIX
        ${INSTALL_DIRECTORY}
        CACHE PATH "Install destination directory.")
else()
    set(CMAKE_INSTALL_PREFIX
        ${CMAKE_CURRENT_LIST_DIR}/../${_project_name}.bin
        CACHE PATH "Install destination directory.")
endif()
    
project(${_project_name} C CXX)
    
# Configure package
configure_file(CMakeLists.txt.in
               ${${_project_name}_download_dir}/CMakeLists.txt)

#
# Download package
#
execute_process(COMMAND ${CMAKE_COMMAND} -G "${CMAKE_GENERATOR}" .
  WORKING_DIRECTORY ${${_project_name}_main_dir}/${${_project_name}_download_dir} )
execute_process(COMMAND ${CMAKE_COMMAND} --build .
  WORKING_DIRECTORY ${${_project_name}_main_dir}/${${_project_name}_download_dir} )

 # Package also has to use one.

add_subdirectory(${${_project_name}_main_dir}/${${_project_name}_source_dir}
                 ${${_project_name}_main_dir}/${${_project_name}_build_dir})
