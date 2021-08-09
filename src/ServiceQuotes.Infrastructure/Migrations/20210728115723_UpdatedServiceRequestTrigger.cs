using Microsoft.EntityFrameworkCore.Migrations;

namespace ServiceQuotes.Infrastructure.Migrations
{
    public partial class UpdatedServiceRequestTrigger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Added use of uuid_generate_v4() for quote id
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION create_quote_for_completed_service()
                RETURNS TRIGGER AS $$
                DECLARE
                    quoteTotal numeric(7,2);
                    materialsTotal numeric(7,2);
                    jobValuationsTotal numeric(7,2);
                BEGIN
                    IF TG_OP = 'UPDATE' THEN
                        IF (OLD.""Status"" = 2 AND NEW.""Status"" = 3) THEN
                            SELECT INTO materialsTotal SUM(m.""UnitPrice"" * m.""Quantity"") FROM ""Materials"" AS m WHERE m.""ServiceRequestId"" = OLD.""Id"";

                            SELECT INTO jobValuationsTotal
                                SUM(
                                    ROUND(
                                        CAST((EXTRACT(HOUR FROM ""LaborHours"") + (EXTRACT(MINUTE FROM ""LaborHours"") / 60)) AS numeric) * ""HourlyRate""
                                    , 2)
                                )
                            FROM ""ServiceRequestJobValuations"" AS srv
                            JOIN ""JobValuations"" AS jv ON srv.""JobValuationId"" = jv.""Id""
                            WHERE srv.""ServiceRequestId"" = OLD.""Id"";

                            quoteTotal := materialsTotal + jobValuationsTotal;

                            INSERT INTO ""Quotes"" (""Id"", ""Total"", ""Status"", ""ServiceRequestId"") VALUES (uuid_generate_v4() ,quoteTotal, 5, OLD.""Id"");
                        END IF;
                    END IF;

                    RETURN new;
                END;
                $$ LANGUAGE 'plpgsql';
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION create_quote_for_completed_service()
                RETURNS TRIGGER AS $$
                DECLARE
                    quoteTotal numeric(7,2);
                    materialsTotal numeric(7,2);
                    jobValuationsTotal numeric(7,2);
                BEGIN
                    IF TG_OP = 'UPDATE' THEN
                        IF (OLD.""Status"" = 2 AND NEW.""Status"" = 3) THEN
                            SELECT INTO materialsTotal SUM(m.""UnitPrice"" * m.""Quantity"") FROM ""Materials"" AS m WHERE m.""ServiceRequestId"" = OLD.""Id"";

                            SELECT INTO jobValuationsTotal
                                SUM(
                                    ROUND(
                                        CAST((EXTRACT(HOUR FROM ""LaborHours"") + (EXTRACT(MINUTE FROM ""LaborHours"") / 60)) AS numeric) * ""HourlyRate""
                                    , 2)
                                )
                            FROM ""ServiceRequestJobValuations"" AS srv
                            JOIN ""JobValuations"" AS jv ON srv.""JobValuationId"" = jv.""Id""
                            WHERE srv.""ServiceRequestId"" = OLD.""Id"";

                            quoteTotal := materialsTotal + jobValuationsTotal;

                            INSERT INTO ""Quotes"" (""Total"", ""Status"", ""ServiceRequestId"") VALUES (quoteTotal, 5, OLD.""Id"");
                        END IF;
                    END IF;

                    RETURN new;
                END;
                $$ LANGUAGE 'plpgsql';
            ");
        }
    }
}
