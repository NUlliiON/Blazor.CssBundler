
# Blazor.CssBundler

Css bundler is a tool to pack css styles of existing razor components into a single css file.

## Warning
This tool is under development. It is not recommended to use it on a working draft.

## Why
Existing version of blazor framework does not have a CSS Isolation.
This tool is recommended to be used together with a component that allows you to isolate CSS (currently under development), otherwise it will not be useful to you.

## Features
 - Bundling razor component project
 - Bundling blazor project
 - Analyzer logic inside this tool allows you to extract css from nested razor components
 - Tool supports emoticons if you have UNIX based terminal or Windows Terminal :D
 - Project switching system for subsequent project assembly
 - Due to the fact that the tool works simply with css files inside the directory of your project, you can use your 		              favorite SASS (SCSS) preprocessor
 - Run the bundler in watch mode. Trigger rebundling on changes in CSS files

## Todo

 - Beta release
 - Interactive project switching system for subsequent project assembly
 - Wiki
 - API
 - Rewrite some parts of the tool functionality

## Screenshots

Bundling razor component project:
![enter image description here](https://i.imgur.com/CBuBi6I.png)

Bundling blazor project:
![enter image description here](https://i.imgur.com/fdZYSOe.png)
