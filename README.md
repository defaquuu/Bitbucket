# Система тестирования знаний

## Сквозной процесс (Этап 7)

Вопросы → Тесты → Ответы → Результаты → Аналитика

## Артефакты

* UML State Machine Diagram
* Логи выполнения сквозного сценария
* Демонстрация успешного сценария
* Ссылка на репозиторий

# Система тестирования знаний
* 
* \## Запуск
* 
* ```bash
* cd TestingSystem.API
* dotnet run --urls=http://localhost:5000

Swagger
http://localhost:5000/swagger

API Endpoints
GET /api/Test/{id} - получить тест

POST /api/Test/start - начать тест

POST /api/Test/submit - отправить ответ

POST /api/Test/complete - завершить тест

GET /api/Test/analytics - аналитика

Тесты