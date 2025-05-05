# Game of Life API

## Overview
The **Game of Life API** is a RESTful service that simulates Conway's Game of Life. It allows users to create, retrieve, and manipulate game boards, calculate the next state of a board, and determine the final state after a series of steps.

## Features
- Add a new game board.
- Retrieve a game board by its ID.
- Calculate the next state of a board.
- Simulate the board's state after a specific number of steps.
- Determine the final state of a board after a maximum number of attempts.

## Technologies
- **.NET 9**
- **C# 13.0**
- **ASP.NET Core** for building the API.
- **xUnit** and **Moq** for unit testing.

---

## Getting Started

### Prerequisites
- .NET 9 SDK installed.
- A Redis server running (if persistence is enabled).

### Installation
1. Clone the repository:

   git clone https://github.com/your-repo/game-of-life-api.git cd game-of-life-api


2. Restore dependencies:

   dotnet restore


3. Build the project:

    dotnet build


4. Run the application:
   
    dotnet run --project GameOfLifeApi



---

## API Endpoints

### 1. **Add a New Board**
- **POST** `/api/GameOfLife/add`
- **Request Body**:
   
{ "rows": 3, "columns": 3, "state": [ [0, 1, 0], [1, 0, 1], [0, 1, 0] ] }

- **Response**:
  - **201 Created**:
    
{
  "BoardId": "123e4567-e89b-12d3-a456-426614174000"
}

  - **400 Bad Request**: Invalid board data.

---

### 2. **Get a Board by ID**
- **GET** `/api/GameOfLife/{boardId}`
- **Response**:
  - **200 OK**:
    
{
  "id": "123e4567-e89b-12d3-a456-426614174000",
  "rows": 3,
  "columns": 3,
  "state": [
    [0, 1, 0],
    [1, 0, 1],
    [0, 1, 0]
  ]
}   

  - **404 Not Found**: Board not found.

---

### 3. **Get the Next State**
- **GET** `/api/GameOfLife/{boardId}/next`
- **Response**:
  - **200 OK**: Returns the next state of the board.
  - **404 Not Found**: Board not found.

---

### 4. **Get State After X Steps**
- **GET** `/api/GameOfLife/{boardId}/steps/{steps}`
- **Response**:
  - **200 OK**: Returns the state of the board after the specified steps.
  - **404 Not Found**: Board not found.

---

### 5. **Get Final State**
- **GET** `/api/GameOfLife/{boardId}/final/{maxAttempts}`
- **Response**:
  - **200 OK**: Returns the final state of the board.
  - **404 Not Found**: Board not found.
  - **400 Bad Request**: Final state not reached within the maximum attempts.

---

## Running Tests
The project includes unit tests for the controllers and services.

1. Navigate to the `tests` directory:
   
   cd GameOfLifeApi/tests


2. Run the tests:
   
   dotnet test


---

## Example Usage
### Add a New Board

curl -X POST http://localhost:5000/api/GameOfLife/add 
-H "Content-Type: application/json" 
-d '{ "rows": 3, "columns": 3, "state": [ [0, 1, 0], [1, 0, 1], [0, 1, 0] ] }'


### Get the Next State
curl -X GET http://localhost:5000/api/GameOfLife/123e4567-e89b-12d3-a456-426614174000/next


---

## Contributing
Contributions are welcome! Please fork the repository and submit a pull request.

---

## License
This project is licensed under the MIT License.

