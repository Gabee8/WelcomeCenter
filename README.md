# Getting Started app for Windows.
[![Version](https://img.shields.io/badge/1.0-passing?label=Release)](https://github.com/Gabee8/WelcomeCenter/releases/tag/release)
[![License](https://img.shields.io/github/license/Gabee8/WelcomeCenter)](https://github.com/Gabee8/WelcomeCenter/blob/main/LICENSE)

Simple Windows Getting Started app program written C# by Visual Studio 2010
##### The program futures:
- 3 style from header
- Add custom apps the list
- Multilanguage enable
##### Edit your language:
Language path: bin\debug\Languages or application root folder\Languages.

Create mylanguage.xaml file example:
```xaml
<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml" xmlns:system="clr-namespace:System;assembly=mscorlib">

    <system:String x:Key="maintitle">Getting Started</system:String>
    <system:String x:Key="welcome">welcome</system:String>
.....
</ResourceDictionary>
```
