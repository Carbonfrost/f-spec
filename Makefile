FRAMEWORK ?= netcoreapp3.0
PREFIX = /usr/local

TEXT_TEMPLATES = dotnet/src/Carbonfrost.Commons.Spec/Automation/Preprocessor/

.PHONY: dotnet/install dotnet/generate -generate-docs -install-manuals

## Install dotnet outputs
dotnet/install: -install-dotnet-fspec -install-manuals-fspec

## Test dotnet
dotnet/test: dotnet/build -dotnet/test

-dotnet/test:
	$(Q) dotnet dotnet/src/fspec/bin/$(CONFIGURATION)/$(FRAMEWORK)/fspec.dll --self-test $(FSPEC_OPTIONS)

## Generate generated code
dotnet/generate:
	$(Q) srgen -c Carbonfrost.Commons.Spec.Resources.SR \
		-r Carbonfrost.Commons.Spec.Automation.SR \
		dotnet/src/Carbonfrost.Commons.Spec/Automation/SR.properties \
		--resx
	$(Q) srgen -c Carbonfrost.CFSpec.Resources.SR \
		-r Carbonfrost.CFSpec.Resources.SR \
		dotnet/src/fspec/Automation/SR.properties \
		--resx
	$(Q) dotnet t4 $(TEXT_TEMPLATES)/GivenExpectationBuilder.tt -o $(TEXT_TEMPLATES)/GivenExpectationBuilder.cs
	$(Q) dotnet t4 $(TEXT_TEMPLATES)/EnumerableExpectations.tt -o $(TEXT_TEMPLATES)/EnumerableExpectations.cs


## Run unit tests with code coverage
dotnet/cover: dotnet/build -check-command-coverlet
	$(Q) coverlet \
		--target "make" \
		--targetargs "-- -dotnet/test" \
		--format lcov \
		--output lcov.info \
		--exclude-by-attribute 'Obsolete' \
		--exclude-by-attribute 'GeneratedCode' \
		--exclude-by-attribute 'CompilerGenerated' \
		--include-test-assembly \
		dotnet/src/fspec/bin/$(CONFIGURATION)/$(FRAMEWORK)/fspec.dll

-install-dotnet-%: -check-env-CONFIGURATION -check-env-FRAMEWORK -check-env-PREFIX
	@ rm -rf $(PREFIX)/opt/$*/
	@ mkdir -p $(PREFIX)/opt/$*/
	@ sed 's/APP_NAME/$*/g' ./dotnet/src/shim.template.sh > $(PREFIX)/bin/$*
	@ chmod +x $(PREFIX)/bin/$*
	@ cp -r ./dotnet/src/$*/bin/$(CONFIGURATION)/$(FRAMEWORK)/ $(PREFIX)/opt/$*/
	@ cp ./dotnet/src/runtimeconfig.template.json $(PREFIX)/opt/$*/$*.runtimeconfig.json

-install-manuals-%: -check-env-PREFIX
	$(Q) install -d $(PREFIX)/opt/$*/share/man/man1/
	$(Q) cp man/man1/$*.1 $(PREFIX)/opt/$*/share/man/man1/
	$(Q) ln -sf $(PREFIX)/opt/$*/share/man/man1/*.1 $(PREFIX)/share/man/man1/

man/man1/fspec.1.html: -generate-docs
man/man1/fspec.1: -generate-docs

-generate-docs:
	$(Q) ronn man/man1/fspec.1.md

include eng/.mk/*.mk
