# lib-rcon
lib-rcon C# Rcon support for minecraft, including using rcon to modify minecraft servers without using mods.

*** This library is being replaced soon, somewhere summer of 2022 into 2023, hopefully ***

This code is delivered as-is.  I would like for it to be help or inspiriation in making your own code, but currently the expectation is to do your own deep dive and be quite familiar with coding.  It is not ment to be a tutorial or a lesson in design.  In fact, this library is horrible for that.  I would recommend getting familar with minecrafts remote console implementation and anything that can be found about the .mca file formats and technical game details to be somewhat researched before using this library.

Even so, I would like to make a more in-depth how-to, some reasons why and various "tutorial-esque" snipits in the future.  This repo isn't ment to be any sort of professional showcase or an attempt to get be popular.  I simply enjoy making things for my minecraft experience (java version) and shared it in case someone else would like a leg-up or can make use of it as well.  This library collection of sorts doesn't have a lot of comments, isn't trying to be white paper standard or anything like that.  All this being said - I do enjoy feedback and I'm working on getting helpful wiki like stuff out in the future.


This library will have support for a asynchronous TCP/IP client connecting to Minecraft's version of RCon allowing for a session , authentication and command transmission/reception. The RCon portion allows for synchronous operation as well.

Additionally, there is support for reading most NBT types to and from streams, and to load and traverse MCA region files. Will be adding the ability to save altered MCA records as well.

A fill rendering system will also be in place that allows for "room" fabrication by sending fill commands to the rconsole and/or sendkeys to a client with an operator player.

Not released, but I do have code that incorperates the output of TOGoS's (http://www.nuke24.net/projects/TMCMR/) Minecraft topography rendering program.
*A quick note, I've created my own map generating routines that are seperate from and does not incorporate TOGos's work - but was part of the history*


Recently added Topography rendering now using my own code in c#.  It does not handle biomes like TOGoS's code.  Process calling into Java to run the java based tool became much too expensive to run (in time, not so much cost) and with the new 1.13.1+ data formats I decided to make my own.

Rcon will now have an Async version, as well as some cleanup when reading stream data.  I discovered a potential for not all bytes being read into an Rcon packet.  
