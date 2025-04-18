# LF.CartonCaps.Referrals.API
This .NET8.0 REST Api handles the Referrals side of the Carton Caps App.

### Client Connection for Fake Datastores
Ideally, this REST Api would connect to LF.CartonCaps.Api for connecting to our Carton Caps Database. However, this example REST Api instead uses two fake datastores for Users and ActiveReferrals. The ActiveReferrals collection provides easier lookup and allows a referee's status to be updated without knowing the userId of who referred them.

### App Integration
When a User of Carton Caps clicks on the `Invite Friends` page, the app calls this REST Api's `GET GetReferrals/{userId}` route, 
which returns all the Referrals the User has made, including FirstName, LastName, and ReferralStatus (Sent, Pending, or Complete).

Users can invite their friends to join Carton Caps by clicking `Share`. The app first calls this REST Api's `POST InviteFriend/{userId}/{referralFirstName}/{referralLastName}` route to create a new Referral on both the User.Referrals list and the ActiveReferrals collection, with a status of Sent. The response is the newly created Referral's ReferralId. The app utilizes a 3rd party deep link service to create a deep link using the ReferralId and the User's ShareableReferralCode, which is already stored within the app.

When a referee clicks the deep link, the deep link service is able to notify the app of the referee's action and the app can in turn can call this API's `PATCH ReferralStatus/{referralId}/{referralStatus}` route to update the referee's status to Pending. Once the app loads for the referee, the app is able to determine if they are a referee by calling the `GET IsReferee/{referralId}` route and show the approprate `Sign Up` page. After the referee has created a new account in the app, the patch route is used again to update the referee's status to Complete and remove this referee from ActiveReferrals.

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
I'll be honest, I didn't know how deep links worked before starting this project and most of what I loosely understand, I've learned from this webpage, [AppsFlyer Deep Linking For Developers](https://www.appsflyer.com/resources/guides/deep-linking-for-developers/). My understanding is a deep linking service would be integrated with the app code and not this underlying REST Api, which is why there's no routes supporting deep linking.

I've put in a basic happy path explaination in the [App Integration](https://github.com/alexissaari/LF.CartonCaps.Referrals.API?tab=readme-ov-file#app-integration) section above. Outside of this, I've done my best to add comments to the tops of files when I think a further explaination of my thought process is needed or would be helpful.
