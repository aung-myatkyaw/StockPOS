#!/bin/bash

# Increment a version string using Semantic Versioning (SemVer) terminology.

# Parse command line options.

while getopts ":Mmprs" Option
do
  case $Option in
    M ) major=true;;
    m ) minor=true;;
    p ) patch=true;;
    r ) release=true;;
    s ) snapshot=true;;
  esac
done

shift $(($OPTIND - 1))

version=$1

# Build array from version string.

a=( ${version//./ } )

# If version string is missing or has the wrong number of members, show usage message.

if [ ${#a[@]} -ne 3 ]
then
  echo "usage: $(basename $0) [-Mmpr] major.minor.patch"
  exit 1
fi

# Increment version numbers as requested.

if [ ! -z $major ]
then
  ((a[0]++))
  a[1]=0
  a[2]=0
fi

if [ ! -z $minor ]
then
  ((a[1]++))
  a[2]=0
fi

if [ ! -z $patch ]
then
  ((a[2]++))
fi

# remove SNAPSHOT from version number
if [ ! -z $snapshot ]
then
  ((a[2]++))
  ((a[2]--))
fi

# remove SNAPSHOT from version number
if [ ! -z $release ]
then
  a[2]=x
fi

echo "${a[0]}.${a[1]}.${a[2]}"
