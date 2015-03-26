# Introduction #

This tool will allow you to merge your [Second Life](http://secondlife.com) chatlog files together. The merge destination can be one of the source paths. And if no destination is provided it will merge and store the merged files in the path specified in the source2 argument.

**Note: secondlife-chatlog-merger is currently only available as a commandline tool**

```
usage:
secondlife-chatlog-merger-1.0.0.exe source1 source2 [destination]

  source1       Chatlog location
  source2       Chatlog location 2 (read destination argument)
  destination   Destination directory of the chatlogs, if not set it will overwrite the files in the path specified by the source2 argument.
```


# Examples #

The Examples below show how you can run secondlife-chatlog-merger. You can either create a shortcut in windows to run the command or run it directly from the commandline.

```
secondlife-chatlog-merger-1.0.0.exe "\\Laptop\C\Users\Administrator\AppData\Roaming\SecondLife\aidamina_hunt" "C:\Users\Administrator\\AppData\\Roaming\SecondLife\aidamina_hunt"
```
To sync any conversations on the "Aidamina Hunt" avatar from the computer \\Laptop to the current machine. This modifies the chatlogs on your local machine.

```
secondlife-chatlog-merger-1.0.0.exe "\\Laptop\C\Users\Administrator\AppData\Roaming\SecondLife\aidamina_hunt" "C:\Users\Administrator\\AppData\\Roaming\SecondLife\aidamina_hunt" "C:\Logs"
```
To sync any conversations on the "Aidamina Hunt" avatar from the computer \\Laptop and current machine to the directory "c:\Logs". This will not modify any chatlogs, just merge them together and copy them to "C:\Logs"

# Download #

[Download secondlife-chatlog-merger-1.0.0.exe](http://code.google.com/p/secondlife-chatlog-merger/downloads)