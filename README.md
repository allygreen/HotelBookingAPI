# HotelBooking

#Intro

This project is a Hotel room booking API, it's designed with a clean architecture, to split the business logic from the data. The API project acts only as input, all validation and business logic is performed by the Application project. The Infrastructure project isolates database access via repositories and EntityFramework contexts. The Core project is responsible for providing DTOs (data transport objects),  database entities and mapping between these objects when data access is required. 


## Table of Contents

• [Features](#features)
• [Architecture](#architecture)
• [Deployment](#deployment)
• [Usage](#usage)


### Deployment

Resources are hosted on the free tier in Azure, the IaC to support this can be found in 
provision/hotelbooking.bicep 

It can be deployed via the following command sequences from within the 'Provisioning' folder.

```
az login 

// create resource group
az group create --name hotelbooking-rg --location "UK South"

// deploy the resources defined in bicep to the resource group
az deployment group create \
  --resource-group hotelbooking-rg \
  --template-file hotelbooking.bicep

// the free plan defined in the bicep is a linux machine, so need to set the startup command
az webapp config set \
  --resource-group hotelbooking-rg \
  --name hotelbooking-admg \
  --startup-file "dotnet HotelBooking.Api.dll"

// get our code compiled and ready for deployment
dotnet publish Src/HotelBooking.Api/HotelBooking.Api.csproj -c Release -o ./publish

// some folder nav + zip
cd publish 

zip -r ../hotelbooking-deploy.zip .

cd ..

// actually deploy the code
az webapp deploy \
  --resource-group hotelbooking-rg \
  --name hotelbooking-admg \
  --src-path ./hotelbooking-deploy.zip \
  --type zip


```


## Features
- Find hotels by name 
- Search available rooms by date and capacity
- Book rooms with validation
- Retrieve booking details
- Data seeding and reset functionality

## Architecture
- Clean Architecture (API/Application/Core/Infrastructure)
- Repository pattern
- Service layer abstraction
- Comprehensive unit testing
- DTOs seperated from database models
- Hotel, Room, Booking tables. 
- A Hotel has a room which has a booking. 
- Booking is made to a Room. 
- EF queries + automapper allow for useful response data
- Application Insights instance (as detailed in bicep file) for Observability 
- Azure SQL Database (uses sqllite locally)


The architecture described can be visualised as below. 

              ┌────────────────────────────---─┐
              │      HotelBooking.API          │
              │  (Controllers / Endpoints)     │
              └──────────────┬─────────---─────┘
                             │
                             ▼
              ┌────────────────────────────---─┐
              │   HotelBooking.Application     │
              │  (Business Logic / Services)   │
              └──────────────┬────────────---──┘
                             │
                             ▼
              ┌───────────────────────────---──┐
              │ HotelBooking.Infrastructure    │
              │(Repositories / EF DbContext)   │
              └──────────────┬─────────────---─┘
                             │
                             ▼
              ┌───────────────────────────---──┐
              │        Database                │
              │  Hotels / Rooms / Bookings     │
              └────────────────────────────---─┘

              ┌───────────────────────────---──┐
              │      HotelBooking.Core         │
              │ (Entities / DTOs / Mapping)    │
              └────────────────────────────---─┘
                     ▲         ▲         ▲
                     │         │         │
              ┌──────┴─────────┴─────────┴──────┐
              │ Referenced by all other layers  │
              └─────────────────────────────────┘


## Usage 

Swagger can be found at 
[/swagger ](https://hotelbooking-admg.azurewebsites.net/swagger/index.html)

The contents below can all be trialled with on swagger at the site but the below is a run through.

The application can be seeded with 5 test hotels using the SeedData API, it has 3 endpoints. 
- Delete All
- Seed 
- Delete & Reseed 

This gives the opportunity to create your own hotels via the hotels endpoint. 

An envisioned workflow is, to call the search hotels endpoint,

/hotels/search/{hotelName}

An example return body is below.

```
[
  {
    "name": "The Prancing Pony",
    "city": "Bree",
    "rooms": [
      {
        "id": 1,
        "capacity": 2,
        "roomType": 1
      },
      {
        "id": 2,
        "capacity": 1,
        "roomType": 0
      },
      {
        "id": 3,
        "capacity": 2,
        "roomType": 2
      },
      {
        "id": 4,
        "capacity": 2,
        "roomType": 1
      },
      {
        "id": 5,
        "capacity": 1,
        "roomType": 0
      },
      {
        "id": 6,
        "capacity": 2,
        "roomType": 2
      }
    ]
  }
]
```

A consuming service could then proceed to make a room booking by taking a selected rooms Id and making a POST request to the booking endpoint. The number of guests in the POST body must be equal or lower than a rooms capacity.

/booking

```
{
  "roomId": 1,
  "customerName": "John Smith",
  "numberOfGuests": 2,
  "checkIn": "2025-11-23T15:00:00Z",
  "checkOut": "2025-11-24T11:00:00Z"
}

```

An example response from the above call would look like 

```
{
    "success": true,
    "message": null,
    "booking": {
        "id": 1,
        "roomId": 1,
        "customerName": "John Smith",
        "numberOfGuests": 2,
        "bookingReference": "HB-GULJUGU4XK",
        "checkIn": "2025-11-23T15:00:00Z",
        "checkOut": "2025-11-24T11:00:00Z",
        "room": {
            "capacity": 2,
            "roomType": 1
        },
        "hotelName": "The Prancing Pony"
    }
}

```

The booking reference contained within the booking can be used to find booking details by being passed into the route of

/booking/details/{bookingReference}

An example response would look like 

```
{
  "success": true,
  "message": null,
  "booking": {
    "id": 2,
    "roomId": 1,
    "customerName": "John Smith",
    "numberOfGuests": 2,
    "bookingReference": "HB-GULJUGU4XK",
    "checkIn": "2025-10-23T15:00:00",
    "checkOut": "2025-10-24T11:00:00",
    "room": {
      "capacity": 2,
      "roomType": 1
    },
    "hotelName": "The Prancing Pony"
  }
}

```

When looking for available rooms, the response is structured by hotel with rooms as list object.

/booking/availability?startDate=2024-01-17&endDate=2024-01-18&guestsCount=2

```
  [
    {
    "hotelId": 1,
    "hotelName": "The Prancing Pony",
    "hotelCity": "Bree",
    "availableRooms": [
      {
        "roomId": 1,
        "capacity": 2,
        "roomType": 1
      },
      {
        "roomId": 3,
        "capacity": 2,
        "roomType": 2
      },
    ]
  
....
  }]
```