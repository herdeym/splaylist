# splaylist
Spotify Playlist Sorter

## Background

[Spotify's API](https://developer.spotify.com) provides a lot of extra information which isn't visible to the user in their web client. Artists can have genre information as specific as 'nu-gaze; aussietronica; deep melodic german house'. Individual tracks also have a machine learning analysis performed on them by Spotify, therefore tracks have a score for variables such as 'dancability', 'acousticness', tempo, key, and time signature. The purpose of Splaylist is to use this API to help sort and categorise a user's playlists. 

To explore the data which Spotify's API holds, check out [MusicalData.com](https://musicaldata.com) or any of the other projects listed on Spotify's developer site.


## Architecture
While initially I was going to use this as an opportunity to learn JavaScript / React, I then discovered that I could create this project in C# with the recently released Blazor framework. Originally, the project was created targeting Blazor WebAssembly, but it has been configured such that it can also run as a Blazor Hosted project (mostly to utilise the Visual Studio debugger).


## Get it running

* Install Blazor through Microsoft's [Get Started guide](https://docs.microsoft.com/en-us/aspnet/core/blazor/get-started).
* Clone this project, making sure to recurse submodules `git clone --recurse-submodules https://github.com/ajmck/splaylist [output directory]`
* Use the [Spotify Developer Dashboard](https://developer.spotify.com/dashboard/) to create a client key.
* Edit `splaylist/splaylist/Config.cs` and populate the above client key.
* Add the callback URI `https://localhost:44326/callback` to the app in Spotify's developer dashboard. 
    * Note that the port number changes depending on if the project is running as WebAssembly or as Hosted; both `Config.cs` and Spotify's developer dashboard must be set with the appropriate link.
* Open `splaylist.sln` in Visual Studio, use the top toolbar to choose between `splaylist` or `splaylist.Hosted`, and use the play button to run the project.
    * Alternatively (and untested), if using the CLI, change directory in to either `splaylist/splaylist` or `splaylist/splaylist.Hosted` and use the command `dotnet run` 

## Issues

* Very early development at this stage; I'll spend some more time to get crucial features implemented then tidy the codebase, and then once an alpha version is deployed online I'll appreciate contributions.

## Resources
* Blazor
* [BlazorDual](https://github.com/ajmck/BlazorDual) - plenty of time spent figuring out how to switch between a WebAssembly and Hosted Blazor project
* [stsrki/Blazorise.DataGrid](https://github.com/stsrki/Blazorise) - DataBound listings
* [JohnnyCrazy/SpotifyAPI-NET](https://github.com/JohnnyCrazy/SpotifyAPI-NET) - .NET library for making requests against Spotify's API, [forked](https://github.com/ajmck/SpotifyAPI-NET) due to Blazor incompatibilities with proxy config / other edits
* [HTML5UP Hyperspace Template](https://html5up.net/hyperspace) - css for prettier homepage
