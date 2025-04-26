# WSTickets

WSTickets is a ticket management system consisting of:
- A **.NET 9 Web API** (WSTickets.Api)
- A **.NET MAUI app** (WSTickets.App)

This system is designed to help employees and clients of Wikibase Solutions register, track, and manage client requests and incidents more efficiently, while supporting ISO27001 requirements for traceability.

## Features

### WSTickets.Api
- Full CRUD (Create, Read, Update, Delete) operations for tickets
- Logging and history per ticket
- User and role management (authentication & authorization)
- Image/file upload functionality linked to tickets
- Push notification support
- Advanced filtering and search
- Built using **Entity Framework Core**
- Ready for hosting (e.g., in Azure)

### WSTickets.App
- Mobile-friendly UI for customers and employees
- Ticket creation, including uploading photos directly from the device's camera
- Status updates and adding comments/logs to tickets
- Filtering tickets based on status and priority
- Searching tickets by client name or subject
- User authentication with optional MFA
- Push notifications for ticket updates
- Account management features (for managers)

## Deployment Targets
- Android (mobile)
- Windows

## Planned Use of Mobile Device Features
- **Camera**: Take pictures of issues and attach them to tickets
- **Geolocation**: Register location of a ticket if relevant
- **Push Notifications**: Notify users about updates to their tickets

## Example User Stories
- As a client, I want to submit a new ticket via my phone.
- As an employee, I want to view and update tickets assigned to me.
- As a manager, I want an overview of who is working on which ticket.
- As a client, I want to attach a photo to a ticket for faster support.
- As a user, I want secure login and possibly MFA.
- And more...

## Getting Started

### Prerequisites
- Visual Studio 2022/2025 with:
  - .NET 9 SDK
  - Mobile development workload (for Android/MAUI)
- Android Emulator or physical Android device
- Postgres database
- (Optional) Azure account for cloud hosting
- (Optional) Git client

## LICENSE

MIT License

Copyright (c) 2025 Jimmy

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
