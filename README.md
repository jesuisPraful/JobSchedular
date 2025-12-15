# Ignition – Job Scheduler (Microservices)

Ignition is a **microservices-based job scheduling system** designed to automate **email and message delivery** using **cron-based scheduling**. It ensures reliable background task execution across distributed services through an **API Gateway** and event-driven communication.

---

## Overview
Ignition allows users to create, manage, and execute scheduled jobs for emails and messages. The system is built for scalability, fault tolerance, and clean service separation.

---

## Services
- **API Gateway** – Single entry point, request routing, authentication
- **Job Scheduler Service** – Cron-based and one-time job execution using Quartz.NET
- **Email Service** – Email delivery, logging, retries, and status tracking
- **Message Service** – Asynchronous message processing via Message Queue

---

## Architecture
- Microservices Architecture
- REST APIs for synchronous communication
- Message Queue for asynchronous communication
- Quartz.NET for scheduling
- Background Services for job execution

---

## Tech Stack
- ASP.NET Core, C#
- REST APIs
- SQL Server
- Quartz.NET
- Async/Await
- Microservices, API Gateway
- Dependency Injection
- Background Services
- LINQ
- Message Queue (RabbitMQ/Kafka)
- Unit Testing (xUnit/NUnit)

---

## Scale of Work
- Developed **100+ backend APIs**
- Managed **hundreds of scheduled jobs**
- Designed a **normalized database with 14+ tables**
- Supported multi-user task scheduling and execution

---

## Execution Flow
1. Client → API Gateway  
2. API Gateway → Job Scheduler Service  
3. Job execution via Quartz.NET  
4. Events published to Message Queue  
5. Email / Message Service processes events  
6. Status logged in database  

---

## Setup
```bash
git clone https://github.com/jesuisPraful/JobSchedular
dotnet restore
dotnet run
