# Project B Server
Game server for client!

WIP

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