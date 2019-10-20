pipeline {
	agent { 
		docker { image 'microsoft/dotnet:3.0-sdk' } 
	}

	stages {
		stage('Build') {
			steps {
				sh 'dotnet restore'
				sh 'dotnet build'
			}
		}
	}
}