FROM mono:4.2

RUN apt-get update && apt-get install -y nuget gtk-sharp2 nunit-console referenceassemblies-pcl

COPY . /code
WORKDIR /code

CMD nuget restore PagarMe.sln \
    && xbuild PagarMe.sln \
    && nunit-console ./PagarMe.Tests/bin/Debug/PagarMe.Tests.dll
