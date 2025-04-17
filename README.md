# LF.CartonCaps.Referrals.API
LiveFront's Code Challenge - Carton Caps Referrals API

Endpoints
> ```http 
> GET /Referrals/{userId}
> ```
> If successful, returns `200 OK` with a list of all Referrals on this User. <br>
> If not successful, return `204 NoContent`
 
> ```http 
> PATCH /Referrals/ReferralStatus/{referralId}/{referralStatus}
> ```
> If successful, returns `200 OK` with a list of all Referrals on this User. <br>
> If not successful, return `400 BadRequest("Failed to update referral status.")`

> ```http 
> POST /Referrals/InviteFriend/{userId}/{referralFirstName}/{referralLastName}
> ```
> If successful, returns `201 Created` with a list of all Referrals on this User. <br>
> If not successful, return `400 BadRequest($"Failed to invite friend {referralFirstName} {referralLastName} to user {userId}")`
