# msfs-bindings

.net8 console application to export Microsoft Flight Simulator 2020 Keyboard and Joystick bindings to csv files.

Set remotePath in appsettings.json to the directory where the inputprofile files are :

![screenshot](https://i.imgur.com/TVYwzlF.png)

For the Steam version, that is something like :

```
"remotePath": "C:\\Program Files (x86)\\Steam\\userdata\\178533733\\1250410\\remote"
```

For the MS store version, that is something like (unable to verify):

```
"remotePath": "C:\\Users\\YOURUSERNAME\\AppData\\Local\\Packages\\Microsoft.FlightSimulator_8wekyb3d8bbwe\\SystemAppData\\wgs"
```

![screenshot](https://i.imgur.com/S32qqTa.png)