# Assignment for week 14 in SSD
This is a small API to demonstrate the principle of least privilege.

## How to Run 

1. cd Week14Security
2. dotnet run --urls="http://localhost:4000"
3. visit `http://localhost:4000/swagger` 

## Quick run-down

There is an endpoint for creating new users, but the database is initialized with users of each category (Editor, Jorunalist, Subscriber). They all have the password `hej`. Usernames can be found by using the `GET` endpoint.

To authenticate as either of the user, utilize the `/api/User/authenticate` endpoint. This will grant you a token. Copy the token.

In swagger there is an "Authorize button" in the top. 

![AuthButton](./Images/auth_1.png)

Use this and type in "Bearer" followed by the token in the value field.  

![AuthBtn2](./Images/auth2.png)

If you are not using swagger, just use the "Bearer <YOUR_TOKEN>" in the header.
