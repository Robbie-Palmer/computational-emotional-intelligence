Copyright(C) <2017>  <Robert Palmer>
This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as
published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
See the GNU General Public License for more details.


This project is the implementation of the goals outlined in "Computational Emotional Intelligence From Facial Expressions of Emotion".
It is submitted in partial fulfilment of the requirements for the degree of BACHELOR OF SCIENCE in Computer Science in The Queen’s
University of Belfast
Robert Palmer
Tuesday 9th May 2017


This project acts as a proof of concept of the utility of facial expressions of emotion as a modality. Its intention is to explore the
entire process of creating useful applications based on this modality to uncover challenges and determine their viability.


The two applications aimed at fulfilling this goal are an Emotionally Aware Chat Bot, and a Key Scene Selector which takes videos of
performances and determines the most important scenes.
The implementation of these projects, and associated projects to aid in their creation and evaluation, has been carried out in C#


The ChatBot folder contains the solution for implementing the Emotionally Aware Chat Bot.


The CognitiveServices folder contains the solution for a library which acts as an interface between the emotionally intelligent
applications, and Microsoft's Cognitive Services APIs.


The KeySceneDataset folder contains the solution for a library that provides access to the videos used to test the Key Scene Selector
models, with associated metadata for assessing their success.
The KeySceneDataset folder additionally contains a project for a console application which evaluates the reliability of the labels
associated with the videos in the dataset.


The KeySceneSelector folder contains the solution for a library, which provides access to a variety of statistical models that attempt
to select important scenes.
The KeySceneSelector folder additionally contains a project for a console application which evaluates the success of the statistical
models.


The Resources folder contains the instances of the dataset.


To run the ChatBot or the console application associated with the KeySceneSelector, ffmpeg must first be downloaded and installed from
https://ffmpeg.org/download.html


Subscription keys are also necessary for a number of Microsoft's Cognitive Services APIs as appropriate.
Namely: Emotion, Bing Speech, Text Anlytics and LUIS.
These can used for free within a certain quota by subscribing at
https://www.microsoft.com/cognitive-services/


The subscription keys and absolute file paths pointing to the datasets in use must be completed in the App.config files within projects
as appropriate for the programs to function.


The external resources used to aid in implementing this project include:

Microsoft Cognitive Services: https://www.microsoft.com/cognitive-services/

Alglib: http://www.alglib.net/

ffmpeg: https://ffmpeg.org/
Open source code developed by J.Burkardt: https://people.sc.fsu.edu/~jburkardt/cpp_src/truncated_normal/truncated_normal.html
Speeches adapted from: http://www.nosweatshakespeare.com/


Details of how to run the applications is found below:


Chat Bot

The ChatBot executable processes all MP4 files in a folder one by one. When each video is processed, it prints the result to console and plays an audio response.
To use this program, ffmpeg must be downloaded and installed from: https://ffmpeg.org/download.html
Subscription keys must be obtained from https://www.microsoft.com/cognitive-services/ for a number of APIs. These are the Emotion API, Bing Speech API and Text Analytics API. The subscription keys must be entered into the ChatBot.exe.config file as the values of Emotion-API-Sub-Key, Speech-API-Sub-Key and Text-Analytics-API-Sub-Key respectively.
File paths to point to the input folder and to point to a location to cache information must be provided to the application. This is done by specifying the values of DatasetFolder and CacheFilePath in the ChatBot.exe.config file.
This program is designed to run on a 64 bit architecture.


Key Scene Dataset Evaluator

The DatasetEvaluator executable accesses the metadata of all the videos specified by the KeySceneDatset project. It evaluates the reliability of the feedback for each video by calculating metrics and outputting the results to files.
A file path pointing to a folder to contain the output files must be specified as the value of EvaluationOutput in the DatasetEvaluator.exe.config file.


Key Scene Selector

The KeySceneSelectorModelEvaluator executable accesses the video data of those deemed reliable from the KeySceneDatset project. It applies a number of models to each video, evaluates the results using metrics and outputs results for each video and on average for a range of model-threshold pairs. 
To use this program, ffmpeg must be downloaded and installed from: https://ffmpeg.org/download.html
Subscription keys must be obtained from https://www.microsoft.com/cognitive-services/ for the Emotion API. The subscription key must be entered into the DatasetEvaluator.exe.config file as the values of Emotion-API-Sub-Key.
The file paths of the folders containing the dataset, for storing cached information and for outputting the analysis must be provided to the program. This is done by specifying the values of DatasetFolder, AllScenesOutput and ModelAnalysisOutput respectively in the DatasetEvaluator.exe.config file. 
This program is designed to run on a 64 bit architecture.
To understand the output of this program, refer to chapter 4.6.1 of the associated dissertation.