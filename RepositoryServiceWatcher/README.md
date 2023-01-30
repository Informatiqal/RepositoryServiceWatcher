# Repository Service Watcher

Small C# Windows service that will make POST request to configured URL when the `Qlik Repository Service` change its status.

Only 2 statuses are monitored - `Running` and `Stopped`. Each of these two will trigger the event.

## Installation

Download the installer from this repository and run it

> **Warning**
>
> The code is not signed and its quite likely that Windows will complain

## Config

- Once installed edit the config file. The default installation location is `C:\Program Files (x86)\Informatiqal\RepositoryServiceWatcher\RepositoryServiceWatcher.exe.config` (open it in any text editor)
- Edit the `url` key `<add key="url" value="http://localhost:8888" />` and replace `http://localhost:8888` with the URL where the request will be made to
- Open Windows service and start `RepositoryServiceWatcher` service

## Request

The request, that the service will made, is `POST` and the body is:

```javascript
{
    "status": "Running"
}
```

OR

```javascript
{
    "status": "Stopped"
}
```
