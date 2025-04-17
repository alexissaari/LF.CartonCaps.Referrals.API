# LF.CartonCaps.Referrals.API
LiveFront's Code Challenge - Carton Caps Referrals API

This .NET8.0 REST Api handles the Referrals side of the Carton Caps App.

### App Integration
When a User of Carton Caps clicks on the `Invite Friends` page, the app calls this REST Api's GetReferrals/{userId} route, which returns all Referrals the User has made.

Users can invite friends to join Carton Caps by clicking `Share`. Call this API to get referralId and send that to deep link software to keep track of

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


