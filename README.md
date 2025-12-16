# Ignition – Job Scheduler

## Project Overview

Ignition is an enterprise-grade, microservices-based job scheduling platform designed to automate email and SMS message delivery with high reliability and scalability. The system enables cron-based task execution across distributed nodes, providing a robust solution for scheduled communication workflows.

## Core Features

### Automated Communication
- **Email Delivery**: SMTP integration for reliable email transmission
- **SMS Messaging**: SMPP protocol support for message delivery
- **Scheduled Execution**: Cron-based job scheduling using Quartz.NET
- **Multi-Channel Support**: Unified interface for managing email and SMS campaigns

### Distributed Architecture
- **Microservices Design**: Independent, scalable service components
- **Message Queue Integration**: Asynchronous inter-service communication
- **API Gateway**: Centralized routing and request management
- **Load Distribution**: Multi-node job execution for high availability

### Enterprise Capabilities
- **Multi-User Support**: Tenant-based task management
- **Job Monitoring**: Real-time tracking of scheduled tasks
- **Failure Handling**: Automatic retry mechanisms and error logging
- **Audit Trail**: Complete history of job executions and outcomes

## Technical Architecture

### Technology Stack

**Backend Framework**
- ASP.NET Core with C#
- REST API architecture
- Async/Await patterns for non-blocking operations
- Dependency Injection for loose coupling

**Data Management**
- SQL Server database
- 14+ normalized tables
- LINQ for data access
- Entity Framework Core ORM

**Job Scheduling**
- Quartz.NET for cron-based scheduling
- Background Services for long-running tasks
- Distributed job execution across nodes

**Communication Protocols**
- SMTP for email delivery
- SMPP for SMS messaging
- Message Queue for service-to-service communication

**Quality Assurance**
- Unit Testing framework
- Integration testing
- Automated test coverage

### Microservices Architecture

The system is composed of multiple independent microservices:

**Core Microservices:**
1. **API Gateway Service** - Request routing and authentication
2. **Job Scheduler Service** - Quartz.NET-based scheduling engine
3. **Email Service** - SMTP-based email delivery
4. **SMS Service** - SMPP-based message delivery
5. **Notification Service** - Multi-channel notification management
6. **User Management Service** - Authentication and authorization
7. **Configuration Service** - System settings and tenant management

```
                    ┌─────────────────┐
                    │   API Gateway   │
                    └────────┬────────┘
                             │
        ┌────────────────────┼────────────────────┐
        │                    │                    │
┌───────▼────────┐  ┌───────▼────────┐  ┌───────▼────────┐
│ Job Scheduler  │  │ Email Service  │  │  SMS Service   │
│   Service      │  │   (SMTP)       │  │   (SMPP)       │
└───────┬────────┘  └───────┬────────┘  └───────┬────────┘
        │                    │                    │
        └────────────────────┼────────────────────┘
                             │
                    ┌────────▼────────┐
                    │  Message Queue  │
                    │   (RabbitMQ/    │
                    │   Azure SB)     │
                    └────────┬────────┘
                             │
        ┌────────────────────┼────────────────────┐
        │                    │                    │
┌───────▼────────┐  ┌───────▼────────┐  ┌───────▼────────┐
│ Notification   │  │ User Mgmt      │  │ Configuration  │
│   Service      │  │   Service      │  │   Service      │
└────────────────┘  └────────────────┘  └────────────────┘
                             │
                    ┌────────▼────────┐
                    │   SQL Server    │
                    │   (14+ Tables)  │
                    └─────────────────┘
```

## Database Schema

The system utilizes a normalized 14+ table database structure including:

**Core Tables**
- **Users**: User accounts and authentication
- **Jobs**: Scheduled job definitions
- **JobExecutions**: Execution history and logs
- **EmailTemplates**: Reusable email templates
- **SMSTemplates**: Reusable SMS templates

**Configuration Tables**
- **ScheduleConfigurations**: Cron expressions and timing
- **SMTPSettings**: Email server configurations
- **SMPPSettings**: SMS gateway configurations
- **APIKeys**: Service authentication

**Operational Tables**
- **JobQueue**: Pending job execution queue
- **FailedJobs**: Failed execution tracking
- **RetryPolicies**: Retry configuration rules
- **AuditLogs**: System activity tracking
- **Notifications**: User notification settings
- **TenantConfigurations**: Multi-tenant settings

## API Architecture

### API Statistics
- **100+ RESTful APIs** covering all system operations
- Comprehensive CRUD operations for all entities
- Bulk operation support for high-volume processing

### Key API Categories

**Job Management APIs (25+ endpoints)**
- Create, update, delete scheduled jobs
- Pause, resume, and trigger jobs manually
- Job status and history retrieval
- Bulk job operations

**Email Service APIs (20+ endpoints)**
- Template management
- Email sending and tracking
- Delivery status reporting
- Attachment handling

**SMS Service APIs (15+ endpoints)**
- Message template CRUD
- SMS dispatch operations
- Delivery confirmation
- Character count and cost estimation

**Scheduling APIs (20+ endpoints)**
- Cron expression management
- Schedule validation
- Timezone handling
- Recurring pattern configuration

**Administration APIs (20+ endpoints)**
- User management
- Tenant configuration
- System health monitoring
- Performance metrics

## Microservices Deep Dive

### 1. API Gateway Service
**Responsibilities:**
- Centralized entry point for all client requests
- Authentication and authorization
- Request routing to appropriate microservices
- Rate limiting and throttling
- Load balancing
- Request/response transformation

**Technologies:** ASP.NET Core, JWT Authentication, Ocelot/YARP

### 2. Job Scheduler Service
**Responsibilities:**
- Core scheduling engine using Quartz.NET
- Cron expression management and validation
- Distributed job execution across nodes
- Job triggering and lifecycle management
- Schedule persistence and recovery
- Job queue management

**Key Features:**
- Cluster-aware scheduling
- Persistent job store
- Automatic failover
- Priority-based execution
- Job chaining support

**Technologies:** Quartz.NET, ASP.NET Core, Background Services

### 3. Email Service
**Responsibilities:**
- SMTP-based email delivery
- Email template management
- Attachment handling
- Delivery status tracking
- Bounce and complaint handling
- Email queue processing

**Integration Points:**
- Receives job triggers from Job Scheduler
- Publishes delivery events to Message Queue
- Stores delivery logs in database

**Technologies:** SMTP Client, MailKit/System.Net.Mail, ASP.NET Core

### 4. SMS Service
**Responsibilities:**
- SMPP protocol integration for SMS delivery
- Message template management
- Character encoding and splitting
- Delivery receipt processing
- Cost calculation and tracking
- Message queue processing

**Integration Points:**
- Receives job triggers from Job Scheduler
- Publishes delivery events to Message Queue
- Maintains delivery status in database

**Technologies:** SMPP Client Library, ASP.NET Core

### 5. Notification Service
**Responsibilities:**
- Multi-channel notification orchestration
- User preference management
- Notification history tracking
- Real-time notification delivery
- Alert and reminder management

**Technologies:** SignalR (optional), ASP.NET Core

### 6. User Management Service
**Responsibilities:**
- User authentication and registration
- Role-based access control (RBAC)
- Token generation and validation
- User profile management
- Multi-tenant user isolation

**Technologies:** ASP.NET Core Identity, JWT, SQL Server

### 7. Configuration Service
**Responsibilities:**
- SMTP/SMPP configuration management
- Tenant-specific settings
- System-wide configuration
- Feature flags
- Dynamic configuration updates

**Technologies:** ASP.NET Core, Configuration Providers

## Inter-Service Communication

### Message Queue Implementation
The system uses a message queue (RabbitMQ/Azure Service Bus) for asynchronous communication:

**Event Types:**
- **JobTriggered**: Scheduler notifies execution services
- **EmailSent**: Email service publishes delivery status
- **SMSSent**: SMS service publishes delivery status
- **JobCompleted**: Execution services notify scheduler
- **JobFailed**: Error notification for retry handling
- **ConfigurationChanged**: Dynamic config updates

**Benefits:**
- Decoupled service architecture
- Guaranteed message delivery
- Load leveling during high traffic
- Event-driven processing
- Resilient communication

### Service-to-Service REST APIs
Direct synchronous communication for critical operations:
- Configuration retrieval
- User authentication validation
- Real-time job status queries
- Immediate feedback requirements

```csharp
// Cron-based scheduling
// Supports complex expressions: "0 0 8 * * ?" (Daily at 8 AM)
// Distributed execution across multiple nodes
// Automatic failover and load balancing
```

**Features:**
- Persistent job store in SQL Server
- Cluster-aware execution
- Job chaining and dependencies
- Priority-based execution

### Asynchronous Processing

All I/O operations utilize async/await patterns:

```csharp
// Non-blocking email sending
// Parallel message processing
// Efficient resource utilization
// Improved throughput
```

**Benefits:**
- Higher concurrency
- Better resource utilization
- Improved system responsiveness
- Scalable under load

### Message Queue Integration

Inter-service communication via message queues:

```csharp
// Decoupled service architecture
// Guaranteed message delivery
// Event-driven processing
// Load leveling
```

**Use Cases:**
- Job status updates
- Email/SMS delivery events
- System notifications
- Audit log generation

### API Gateway Pattern

Centralized API management:

```csharp
// Single entry point for all requests
// Authentication and authorization
// Request routing and load balancing
// Rate limiting and throttling
```

**Advantages:**
- Simplified client integration
- Centralized security
- Traffic management
- Service discovery

## Scale and Performance

### System Metrics
- **Jobs Managed**: Hundreds of concurrent scheduled jobs
- **API Endpoints**: 100+ RESTful APIs
- **Database**: 14+ normalized tables
- **Daily Throughput**: Thousands of emails and messages
- **Execution Nodes**: Multi-node distributed system

### Performance Optimizations
- Connection pooling for database operations
- Cached configuration for frequent lookups
- Batch processing for bulk operations
- Async processing for I/O operations
- Query optimization with proper indexing

## Reliability Features

### Failure Handling
- **Automatic Retries**: Configurable retry policies for failed jobs
- **Dead Letter Queue**: Failed job isolation and analysis
- **Circuit Breaker**: Prevents cascading failures
- **Graceful Degradation**: System continues operating during partial failures

### Monitoring and Logging
- **Execution Logs**: Detailed job execution history
- **Performance Metrics**: Response times and throughput tracking
- **Error Tracking**: Comprehensive error logging and alerting
- **Health Checks**: Service availability monitoring

### Data Integrity
- **Transactional Operations**: ACID compliance for critical operations
- **Optimistic Concurrency**: Prevents data conflicts
- **Audit Trail**: Complete change history
- **Backup and Recovery**: Regular automated backups

## Security Implementation

### Authentication & Authorization
- JWT token-based authentication
- Role-based access control (RBAC)
- API key management for service-to-service calls
- Multi-tenant data isolation

### Data Protection
- Encrypted sensitive data at rest
- TLS/SSL for data in transit
- Secure credential storage
- Input validation and sanitization

## Distributed Architecture Benefits

### Independent Scalability
Each microservice can scale independently based on load:
- Scale Email Service during marketing campaigns
- Scale SMS Service during peak messaging hours
- Scale Job Scheduler during high scheduling activity

### Technology Flexibility
Each service can use optimal technology:
- Different database strategies per service
- Service-specific caching mechanisms
- Optimized for specific workload patterns

### Fault Isolation
Failures in one service don't cascade:
- Email service downtime doesn't affect SMS
- Job scheduling continues if notification service fails
- Graceful degradation of non-critical services

### Development Efficiency
Teams can work independently:
- Separate codebases per service
- Independent deployment cycles
- Service-specific testing strategies
- Technology stack flexibility

## Deployment Architecture

### Microservices Deployment
Each service is deployed independently:

**Containerization:**
- Docker containers for each microservice
- Container orchestration (Kubernetes/Docker Swarm)
- Service discovery and registration
- Health monitoring per service

**Deployment Strategy:**
- Blue-green deployments
- Rolling updates
- Zero-downtime deployments
- Independent service versioning

**Infrastructure:**
- Load balancers per service
- Auto-scaling based on metrics
- Database per service pattern (where applicable)
- Shared database with schema separation

### Configuration Management
- Environment-specific configurations
- External configuration sources
- Secret management integration
- Dynamic configuration updates

## Development Practices

### Code Quality
- **Unit Testing**: Comprehensive test coverage
- **Integration Testing**: End-to-end workflow validation
- **Code Reviews**: Peer review process
- **Static Analysis**: Automated code quality checks

### Design Patterns
- **Dependency Injection**: Loose coupling and testability
- **Repository Pattern**: Data access abstraction
- **Factory Pattern**: Object creation flexibility
- **Strategy Pattern**: Algorithm encapsulation

## Use Cases and Benefits

### Business Applications
- Marketing campaign automation
- Transactional email delivery
- SMS notification systems
- Reminder and alert services
- Report generation and distribution

### Key Benefits
- **Reliability**: Guaranteed job execution with retry mechanisms
- **Scalability**: Handles growing workloads seamlessly
- **Flexibility**: Supports complex scheduling patterns
- **Maintainability**: Clean architecture and separation of concerns
- **Visibility**: Comprehensive monitoring and logging

## Future Enhancements

### Planned Features
- Real-time dashboard for job monitoring
- Advanced analytics and reporting
- Webhook support for external integrations
- GraphQL API support
- Machine learning for optimal scheduling

### Scalability Roadmap
- Kubernetes orchestration
- Event sourcing implementation
- CQRS pattern adoption
- Multi-region deployment support

---

## Technical Highlights

This project demonstrates expertise in:

- **Enterprise Architecture**: Microservices design and implementation
- **Distributed Systems**: Multi-node job scheduling and coordination
- **Asynchronous Programming**: Efficient async/await patterns
- **Database Design**: Normalized schema with 14+ tables
- **API Development**: 100+ RESTful endpoints
- **Integration**: SMTP, SMPP, Message Queue protocols
- **Testing**: Unit and integration test coverage
- **Performance**: Optimized for high-volume operations
- **Reliability**: Failure handling and retry mechanisms
- **Security**: Authentication, authorization, and data protection
