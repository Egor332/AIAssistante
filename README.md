# Google Gemini Assistant Web Application

This project is designed to interact with Google's Gemini model, featuring:
- **Back-end**: ASP.NET Web API
- **Front-end**: React (TypeScript) with Vite

---

## Cloning the Repository

First, clone the repository to your local machine:

```sh
git clone https://github.com/Egor332/AIAssistante
cd AIAssistante
```

## Project Structure

/BackEnd → ASP.NET Web API project

/FrontEnd → React + Vite front-end project

docker-compose.yml → For running the entire stack via Docker


---

## Running the Application via Docker

Follow the steps below to run the entire application using Docker Compose.

### 1. Prepare `appsettings.json`

Navigate to the `/BackEnd` folder and create a file named `appsettings.json`.  
Use `appsettings.Example.json` as a reference for the required structure.

At minimum, make sure to fill in the following environment variables:
- `LLM:ApiKey` → Your API key for the Google Gemini model
- `LLM:curl` → Your curl command or endpoint for the model

> **Note:** You do **not have to**  set the `AllowedOrigins` field when running with Docker Compose — this value will be overridden automatically.

### 2. Start the Application

Open a terminal in the **main project directory** and run:

```sh
docker-compose up -d
```
Once the containers are running, open your browser and go to:
http://localhost:5173

You can use the chat on the front-end to communicate with the assistant.

> **Note:** Sometimes it takes a few seconds for the back-end to start, and it may not respond to a message within the first few seconds. You can try again a few seconds later. 
