# LF.CartonCaps.Referrals.API
This .NET8.0 REST Api handles the Referrals side of the Carton Caps Mobile App.

### Client Connection for Fake Datastores
Ideally, this REST Api would connect to LF.CartonCaps.Api for connecting to our Carton Caps Database. However, this example REST Api instead uses two fake datastores for Users and ActiveReferrals. The ActiveReferrals collection provides easier lookup and allows a referee's status to be updated without knowing the UserId of who referred them.

### System Diagram
![LF CartionCaps Referrals System Diagram drawio](https://github.com/user-attachments/assets/d340919e-3613-4ce2-9c28-8cd806732c03)

### Endpoints
> ```http 
> GET /Referrals/{userId}
> ```
> If successful, returns `200 OK` with a list of all the User's Referrals <br>
> If User.Referrals is null or empty, returns `204 NoContent` <br>
> If User does not exist, throws `UserDoesNotExistException` <br>

> ```http 
> GET /IsReferral/{userId}
> ```
> If successful, returns `200 OK` with a boolean value for if the person has been refereed <br>
> Note: If ActiveReferral does not exist for this person, return false instead of throwing an exception
 
> ```http 
> PATCH /Referrals/ReferralStatus/{referralId}/{referralStatus}
> ```
> If successful, returns `200 OK` <br>
> If not successful, return `400 BadRequest` <br>
> If User does not exist, throws `UserDoesNotExistException` <br>
> If ActiveReferral does not exist, throws `ActiveReferralDoesNotExistException` <br>
> If Referral does not exist on User, throws `ReferralDoesNotExistOnUserException`

> ```http 
> POST /Referrals/InviteFriend/{userId}/{referralFirstName}/{referralLastName}
> ```
> If successful, returns `201 Created` with a list of all Referrals on this User. <br>
> If not successful, return `400 BadRequest` <br>
> If User does not exist, throws `UserDoesNotExistException` <br>
> If ActiveReferral does not exist, throws `ActiveReferralDoesNotExistException` <br>
> If Referral does not exist on User, throws `ReferralDoesNotExistOnUserException`

### Personal Notes
I'll be honest, I didn't know how deep links worked before starting this project. Most of what I now loosely understand, I've learned from this webpage: [AppsFlyer - Deep Linking For Developers](https://www.appsflyer.com/resources/guides/deep-linking-for-developers/). My understanding is a deep linking service would be integrated directly into the app's codebase and not this underlying REST Api, which is why there's no routes supporting deep linking.

I've put in a basic happy path explaination in the [App Integration](https://github.com/alexissaari/LF.CartonCaps.Referrals.API?tab=readme-ov-file#app-integration) section above. Outside of this, I've done my best to add comments to the tops of files when I think a further explaination of my thought process is needed or would be helpful.

#### Extras: <br>
All code changes (non-README) were commited from branches through pull requests and not directly against main. <br>
I was able to run Acceptance Tests by running two instances of Visual Studio, one running the service itself and the other running the Acceptance Tests that call the running service's localhost. <br>
