# Insight Airport

## Solution Composition

This solution was developed taking in consideration the hexagonal architecture.
Due this fact this project has no dependency between the business rules and the Database code related.
The web api project is responsible to exposa all the functionalities to be consumed by an application or other service.

Beyond that was used the docker-compose file to aggregate all the applications (Web-Api, DB and the Worker) and run over the orchestrator.

This project is composed by
- A core where all the business rules should be implemented
- A infra (right port) where is implemented all the communication between the business and the database
- A api (left port) where is exposed the functionalities to the world
- Migrations, where all the migrations generated will be placed
- Finally the worker, which is responsible to simulate the usage of the api for a external service.

## Missing parts

It's missing the UI to display the data from the api consitently and the increase of the code coverage, all of this still in progress.

