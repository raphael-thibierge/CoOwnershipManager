START TRANSACTION;

ALTER TABLE "AspNetUsers" ADD "ApartmentId" integer NULL;

CREATE INDEX "IX_AspNetUsers_ApartmentId" ON "AspNetUsers" ("ApartmentId");

ALTER TABLE "AspNetUsers" ADD CONSTRAINT "FK_AspNetUsers_Apartments_ApartmentId" FOREIGN KEY ("ApartmentId") REFERENCES "Apartments" ("Id") ON DELETE SET NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20210216044917_ApartmentHasManyUnhabitants', '5.0.3');

COMMIT;

