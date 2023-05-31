TRUNCATE TABLE inquiries."Inquiries" CASCADE;

TRUNCATE TABLE sales."Offers";
TRUNCATE TABLE sales."Reservations";
TRUNCATE TABLE sales."DeadlineRegistryEntries";

TRUNCATE TABLE workloads."Workloads";

TRUNCATE TABLE backoffice."Documents";

TRUNCATE TABLE "time-management"."Deadlines" CASCADE;
TRUNCATE TABLE "time-management"."Notifications";
TRUNCATE TABLE "time-management"."DeadlineParticipant";

TRUNCATE TABLE finance."Payers";
TRUNCATE TABLE finance."Payments";

TRUNCATE TABLE postsale."ChangeRequests" CASCADE;
TRUNCATE TABLE postsale."ReservationSnapshots" CASCADE;
TRUNCATE TABLE postsale."ChangesToApply" CASCADE;
