rm -rf ./TestReports

dotnet test \
    --results-directory ./TestReports \
    --collect:"XPlat Code Coverage;Format=json,cobertura" \
    /p:DeterministicSourcePaths=true -- \
    DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.DeterministicReport=true

cp ./TestReports/*/coverage.cobertura.xml TestReports/coverage.cobertura.xml
reportgenerator -reports:"./TestReports/coverage.cobertura.xml" -targetdir:"./CoverageReport" -reporttypes:"Html_Dark;Badges"
rm -rf ./TestReports
open ./CoverageReport/index.htm
