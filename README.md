# LF.CartonCaps.Referrals.API
This .NET8.0 REST Api handles the Referrals side of the Carton Caps App.

### Client Connection for Fake Datastores
Ideally, this REST Api would connect to LF.CartonCaps.Api for connecting to our Carton Caps Database. However this example REST Api instead uses two fake datastores for Users and ActiveReferrals. The ActiveReferrals collection provides easier lookup and allows a referee's status to be updated without knowing the userId of who referred them.

### App Integration
When a User of Carton Caps clicks on the `Invite Friends` page, the app calls this REST Api's `GET GetReferrals/{userId}` route, 
which returns all the Referrals the User has made, including first and last names and referral status (Sent, Pending, or Complete).

Users can invite their friends to join Carton Caps by clicking `Share`. The app first calls this REST Api's `POST InviteFriend/{userId}/{referralFirstName}/{referralLastName}` route to create a new Referral on both the User.Referrals list and the ActiveReferrals collection, with a status of Sent. The response is the newly created Referral's ReferralId. The app utilizes a 3rd party deep link service to create a deep link using the ReferralId and the User's ShareableReferralCode, which is already stored within the app.

When a referee clicks the deep link, the deep link service is able to notify the app of the referee's action and the app can in turn can call this API's `PATCH ReferralStatus/{referralId}/{referralStatus}` route to update the referee's status to Pending. Once a referee has created a new account in the app, this same patch route is used to update the referee's status to Complete.


### Endpoints
> ```http 
> GET /Referrals/{userId}
> ```
> If successful, returns `200 OK` with a list of all the User's Referrals <br>
> If User.Referrals is null or empty, returns `204 NoContent` <br>
> If User does not exist, throws `UserDoesNotExistException` <br>
 
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


