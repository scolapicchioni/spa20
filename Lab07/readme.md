# Security: Resource Based Authorization



[Resource Based Authorization](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/resourcebased?tabs=aspnetcore2x)

On the API
- Add UserName to Product Model
- Add AuthorizationService on controller constructor DI
- Invoke authorizationservice on Update
- Register Policy, Requirement and Handler on Startup
- Create Requirement and Handler 
- Add Migration

On the Client
- Add UserName to product
- Show update / delete buttons only if user is owner
 
Go to `Labs/Lab08`, open the `readme.md` and follow the instructions thereby contained.   