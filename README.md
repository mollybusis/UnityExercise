# Unity Exercise 

Clone this Github project and expand it by adding a new game to it.

The project is made in Unity 2022.3

It implements a very simple game called React, which briefly displays a stimulus (rectangle) and the player has to respond as quickly as possible.

The codebase is structured so that more complex games can be built on top of it easily and its behavior can be controlled using an xml parameter file (which we call the session file).
See the 'Overview' section below for more details on the structure of the project.


Once you're familiar with the game and how the code is organized, you will be implementing one new game in this project.
In this game you should be able to:

- Specify in the session file whether the stimulus (rectangle) position per trial is random or predefined.
  - The predefined position should be defined inside the session file on a per trial basis. 
  - While the random position should be generated based off a defined range.
- Specify in the session file whether the stimulus should sometimes appear red, in which case the player should NOT respond in order to get a correct response.
- Save all the new game parameters (position, isRed, etc...) when creating a session log file at the end of a game.
- Log important events such as the result of each Trial to a trace file by utilizing the GUILog functionality that the project has, instead of Unity's Debug.Log


# Things to keep in mind

- Treat this exercise as a real world scenario where we ask you to add a new game to our existing project.
- The original React game should remain unchanged.
- Keep object oriented programming concepts, like polymorphism and inheritance, in mind.
- The new code should maintain the formatting conventions of the original code.
- You will need to make a new session file for your new game.
- You really don't need to delve too deep into how the XML serialization works. This page should give you a better idea of how XML works:
    - https://www.w3schools.com/xml/xml_tree.asp
- Look at the way that the XMLUtil methods CreateAttribute() and ParseAttribute() are called.
- Make sure that the new session file's settings element is tailored to your new game.
    - SessionData.cs should write out the correct gameType for the general element.
    - Make sure that SessionUtil.GetSessionGameElement() returns the correct string for your new game.


# Project Overview

- **Stimulus** - An event that a player has to respond to.
- **Session** - A session refers to an entire playthrough of a game.
- **Trial** - A trial is when a player has to respond to a stimulus, which becomes marked as a success or failure depending on the player's response.
- **TrialResult** - A result contains data for how the player responded during a Trial.
- **Session File** - A session file contains all the Trials that will be played during a session, as well as any additional variables that allow us to control and customize the game.
- **Session Log** - An xml log file generated at the end of a session, contains all the attributes defined in the source Session file as well as all the Trial results that were generated during the game session.
- **Trace Log** - A text log file generated using GUILog for debugging and analytical purposes. GUILog requires a SaveLog() function to be called at the end of a session in order for the log to be saved.
- **GameController** - Tracks all the possible game options and selects a defined game to be played at the start of the application.
- **InputController** - Checks for player input and sends an event to the Active game that may be assigned.
- **GUILog** - A trace file logging solution, similar to Unity's Debug.Log, except this one creates a unique log file in the application's starting location.
- **GameBase** - The base class for all games.
- **GameData** - A base class, used for storing game specific data.
- **GameType** - Used to distinguish to which game a Session file belongs to.


# Submission

For your submission, extend this README documenting the rules of the new game, how the code works, how scoring works in the new game, and any other interesting or useful things you can think of for us to take into consideration. Then zip the git repository and send it to us.


# New game: Detect

My new version of the game is called "Detect". This seemed appropriate given the new rules: when red rectangles (those that should be ignored) appear among white ones, the user must not only react to a stimulus but also *detect* its color to determine whether to respond.

## Setup

Several new settings have been added to be set in the Session File. An XML file for playing Detect can be set up with all the same settings options as React, but with the following additions:
- General settings
  - **gameType**: must be "Detect"
- Detect settings
  - **randomPositions**: boolean indicating whether to randomize the position of the stimulus in each trial. Optional; default false.
  - **includeRed**: boolean indicating whether to include red stimuli (to be ignored) in the game. Optional; default false.
  - **positionRangeMinX**: Minimum inclusive x-value for random positions. Optional; default 0.
  - **positionRangeMaxX**: Maximum exclusive x-value for random positions. Optional; default 0.
  - **positionRangeMinY**: Minimum inclusive y-value for random positions. Optional; default 0.
  - **positionRangeMaxY**: Maximum exclusive y-value for random positions. Optional; default 0.
- Trials
  - **positionX**: x-value for stimulus position. Only used if **randomPositions** is false. Optional; default 0.
  - **positionY**: y-value for stimulus position. Only used if **randomPositions** is false. Optional; default 0.
  - **isRed**: boolean indicating whether this trial's stimulus will be red (to be ignored by the user). Only used if **includeRed** is true. Optional; default false.
  
## Rules

The rules of Detect are quite similar to those of React. The user waits for stimuli (rectangles) and presses the spacebar to react to them. White rectangles should be reacted to, but red ones should be ignored. Pressing the spacebar when a red stimulus is on screen results in a failed response. For white stimuli, as before, a response is successful only if the rectangle is onscreen and the spacebar is pressed within the Guess Limit and Response Limit times.

## Scoring

The output for Detect is formatted the same as that for React but with the new Session Data settings added. Within the results, accuracy for red stimuli is listed as 1 if the stimulus is ignored and 0 if it is not. Reaction time is still listed for red stimuli, appearing as the time taken before a response if the trial was a failure and 0 if it was a success (the stimulus was ignored).

## About the code

- I began by creating three new classes for my game: Detect, DetectData, and DetectTrial.
  - They are subclasses of GameBase, SessionData, and Trial, respectively.
  - Although I considered inheriting from the React classes (my reasoning being that Detect is built off of React), I realized that this would complicate things, and I would end up having to override and rewrite most of the methods anyway.
  - I ended up reusing a lot of the React code but with various additions for the functionality of Detect.
- Some time was spent figuring out how to correctly read in and account for my new settings in the XML file. I had to add my new gameType as a case to a couple of switch statements to get things working. Now, the new settings are read in DetectData and DetectTrial.
- I then added the actual new elements to the game. This mostly ended up being in the Detect file. I made a second rectangle game object to act as the red ("bad") stimulus; the game switches between which to display depending on isRed for the trial and includeRed for the session. Cases were added to the success conditions to account for the red stimuli (which should be ignored).
- The position randomization is done in DetectTrial; upon reading in position data, if the session has randomPositions set to true, the coordinates are re-assigned to random values using the provided or default ranges.
- I made sure that the original React game is still playable and that Detect works correctly with various settings configurations.

## Updates 11/7

- After Robin's message about taking more advantage of polymorphism, I have gone through and made all three Detect classes into subclasses of their corresponding React classes.
    - There was, as expected, a lot of copied-over code that I was able to delete from my Detect classes. When I initially wrote them, I was focused more on making everything function correctly than structuring it in a way that made sense in terms of inheritance. I believe it should be fixed now.
- I did end up editing the React file - I changed the access of the constants and DisplayFeedback in React to be protected.
    - I had been re-declaring the constants in Detect, which Unity did not allow and seemed unnecessary. I could have declared them with different names, but accessing them from the parent class better takes advantage of being a subclass of it. I did the same for DisplayFeedback, making it protected so Detect could use it.
    - Because changing these to protected in React does not change the functionality of the React game, I reasoned that it should be a valid thing to do.
    - If this was a mistake, please let me know and I can change them all back to private as before. (But if so, I would be curious to know why these need to be private rather than protected!)
