# SenseCapitalTestTask


## Анонимные маршруты
### Путь: /api/Auth/Login
Описание: Служит для авторизации пользователя, если пользователь существует и ввел корректные данные, возвращется JWT токен  
Метод запроса: POST  
Тело запроса:  
```
{  
  "username":	"string" //Мин. 8 Макс. 16 символов  
  "password":	"string" //Мин. 8 Макс. 16 символов  
}  
```
Тело ответа: "string"  
### Путь: /api/Auth/Register
Описание: Служит для регистрации пользователя, возвращет JWT токен  
Метод запроса: POST  
Тело запроса:  
```
{  
  "username":	"string" //Мин. 8 Макс. 16 символов  
  "password":	"string" //Мин. 8 Макс. 16 символов  
}  
```
Тело ответа: "string"  
## Маршруты требующие авторизацию
### Путь: /api/GameSessions  
Описание: Выводит список незавершенных игровых сессий  
Метод запроса: GET  
Тело ответа:  
```
[
  {
    "id": 0,
    "firstPlayerId": 0,
    "firstPlayerSide": 0,
    "secondPlayerId": 0,
    "secondPlayerSide": 0,
    "playerTurnId": 0,
    "isGameEnded": true,
    "winnerId": 0,
    "gameBoard": "string"
  }
]
```
### Путь: /api/GameSessions/{id:int}  
Описание: Выводит незавершенную игровую сессию по ее id   
Метод запроса: GET  
Тело ответа:  
```
  {
    "id": 0,
    "firstPlayerId": 0,
    "firstPlayerSide": 0,
    "secondPlayerId": 0,
    "secondPlayerSide": 0,
    "playerTurnId": 0,
    "isGameEnded": true,
    "winnerId": 0,
    "gameBoard": "string"
  }
```
### Путь: /api/GameSessions/ 
Описание: Создается пустая сессия с авторизованным игроком и выбранной им стороной   
Метод запроса: POST  
Тело запроса:  
```
{
  "playerSide": 0 // Тип Enum, 0 - Крестики, 1 - Нолики
}
```
Тело ответа:  
```
  {
    "id": 0,
    "firstPlayerId": 0,
    "firstPlayerSide": 0,
    "secondPlayerId": 0,
    "secondPlayerSide": 0,
    "playerTurnId": 0,
    "isGameEnded": true,
    "winnerId": 0,
    "gameBoard": "string"
  }
```
### Путь: /api/GameSessions/JoinToSession/{id:int}
Описание: Подключение к пустой сесии по ее id   
Метод запроса: PUT  
Тело ответа:  
```
  {
    "id": 0,
    "firstPlayerId": 0,
    "firstPlayerSide": 0,
    "secondPlayerId": 0,
    "secondPlayerSide": 0,
    "playerTurnId": 0,
    "isGameEnded": true,
    "winnerId": 0,
    "gameBoard": "string"
  }
```
### Путь: /api/GameSessions/MakeMove  
Описание: Пользователь делает свой ход     
Метод запроса: PUT  
Тело запроса:  
```
{
  "matrixX": 2,
  "matrixY": 2,
  "sessionId": 0
}
```
Тело ответа:  
```
  {
    "id": 0,
    "firstPlayerId": 0,
    "firstPlayerSide": 0,
    "secondPlayerId": 0,
    "secondPlayerSide": 0,
    "playerTurnId": 0,
    "isGameEnded": true,
    "winnerId": 0,
    "gameBoard": "string"
  }
```




















