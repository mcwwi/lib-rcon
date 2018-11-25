# lib-rcon
lib-rcon C# Rcon support for minecraft, including using rcon to modify minecraft servers without using mods.

This library will have support for a asynchronous TCP/IP client connecting to Minecraft's version of RCon allowing for a session , authentication and command transmission/reception. The RCon portion allows for synchronous operation as well.

Additionally, there is support for reading most NBT types to and from streams, and to load and traverse MCA region files. Will be adding the ability to save altered MCA records as well.

A fill rendering system will also be in place that allows for "room" fabrication by sending fill commands to the rconsole and/or sendkeys to a client with an operator player.

Not released, but I do have code that incorperates the output of TOGoS's (http://www.nuke24.net/projects/TMCMR/) Minecraft topography rendering program.

Recently added Topography rendering now using my own code in c#.  It does not handle biomes like TOGoS's code.  Process calling into Java to run the java based tool became much too expensive to run (in time, not so much cost) and with the new 1.13.1+ data formats I decided to make my own.

Rcon will now have an Async version, as well as some cleanup when reading stream data.  I discovered a potential for not all bytes being read into an Rcon packet.  
