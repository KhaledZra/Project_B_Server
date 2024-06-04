# Project B Server
Game server for client!

If you wish to play the game the client repo is here: https://github.com/KhaledZra/Project_B_Client

This is the server that helps my clients connect and communicate with each other with the help of SignalR.

#### Cool features:
- Fully working CI/CD pipeline with github actions
- Deployed with help of Docker to my Heroku cloud
- MongoDb to store client information
- Xunit unit test coverage for my services. Unfortunatly at the time i was working on this SignalR/Blazor has breaking changes in regards being Unit testable.
- Not relevant but a cool live chat thanks to Microsoft Docs that i built on a little. Was going to add this to my Client to help Players communicate with each other. But my deadline was getting closer and closer. :(

## server url:
https://project-b-server-081b429cac7e.herokuapp.com/

## Development docker and heroku commands

### Building the docker image
docker build . -t project-b-server-heroku-app

### pushing image
heroku container:push web -a project-b-server

### releasing image
heroku container:release web -a project-b-server

### see logs
heroku logs --tail -a project-b-server


### very good tutorial
https://medium.com/swlh/deploy-your-net-core-3-1-application-to-heroku-with-docker-eb8c96948d32
https://devcenter.heroku.com/articles/heroku-cli-commands
