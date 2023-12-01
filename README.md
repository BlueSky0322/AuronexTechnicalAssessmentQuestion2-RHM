# Random Hash Machine (RHM) API Service
![image](https://github.com/BlueSky0322/AuronexTechnicalAssessmentQuestion2-RHM/assets/60435524/ce6f755b-cfa3-4ee7-a2f2-efbced3df04b)


# Introduction
The Random Hash Machine (RHM) API is a system designed to generate and verify random hash strings using the SHA-256 algorithm. The API consists of two endpoints.

# Features
### Endpoint 1
- This endpoint generates a unique hash string using the SHA-256 algorithm.
- Each hash string generated is unique and not repeatable.
- The endpoint has a deliberate 1-second delay before returning a response.

### Endpoint 2
- This endpoint continuously requests hash strings from Endpoint #1.
- It returns a success response only when the last character of the hash string received from Endpoint #1 is both a number and an odd number.


# Technologies Used
- ASP.NET Core Web API with C# using Microsoft Studio 2022
- SwaggerUI for API testing
- Visual Studio Installer for modifying installations

# Load Testing Objective
The objective of this load test is to evaluate and validate the performance and reliability of Endpoint #2 in the RHM API. Specifically:
### Endpoint #2 Functionality
- Verify if Endpoint #2 successfully identifies the hash strings meeting the specific criteria (last character being a number and an odd number).

### Request Rate Testing and Load Handling
- Assess how Endpoint #2 handles consistent requests at a steady rate over a sustained period.
- Test the performance of Endpoint #2 by sending 1 request per second.
