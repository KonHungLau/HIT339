**1. ApplicationUser Model (inherits from IdentityUser):**
   - FirstName (string)
   - LastName (string)
   - IdentityRole (string) // "Member", "Coach", or "Admin". Comes bundled with IdentityUser
   - Biography (string)
   - Schedules (ICollection\<Schedule\>)
   - Enrollments (ICollection\<Enrollment\>)
   
**3. Schedule Model:**
   - ScheduleId (int, primary key)
   - EventName (string)
   - Date (DateTime)
   - Location (string)
   - MaxParticipants (int) // Maximum number of participants allowed
   - CoachId (string, foreign key)
   - Coach (Coach)
   - Enrollments (ICollection\<Enrollment\>)

**4. Enrollment Model:**
   - EnrollmentId (int, primary key)
   - MemberId (string, foreign key)
   - Member (Member)
   - ScheduleId (int, foreign key)
   - Schedule (Schedule)
   - EnrollmentDate (DateTime) // Date when the member enrolled

**5. SpecialOffer Model:**
   - SpecialOfferId (int, primary key)
   - Title (string)
   - Description (string)
   - StartDate (DateTime)
   - EndDate (DateTime)
   - TargetAudience (string) // "Members", "Coaches", or "Both"
